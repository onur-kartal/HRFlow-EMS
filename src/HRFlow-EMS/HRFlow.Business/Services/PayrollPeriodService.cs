using AutoMapper;
using HRFlow.Business.DTOs.Logging;
using HRFlow.Business.DTOs.Payroll;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Constants;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.Enums;
using HRFlow.Entities.HumanResources;
using System.Globalization;

namespace HRFlow.Business.Services
{
    public class PayrollPeriodService : IPayrollPeriodService
    {
        private readonly IPayrollPeriodRepository _payrollPeriodRepository;
        private readonly IEmployeePayrollRepository _employeePayrollRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public PayrollPeriodService(
            IPayrollPeriodRepository payrollPeriodRepository,
            IEmployeePayrollRepository employeePayrollRepository,
            IAuditLogService auditLogService,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _payrollPeriodRepository = payrollPeriodRepository;
            _employeePayrollRepository = employeePayrollRepository;
            _auditLogService = auditLogService;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<List<PayrollPeriodListDto>> GetListAsync()
        {
            var payrollPeriods = await _payrollPeriodRepository.GetListAsync();

            return _mapper.Map<List<PayrollPeriodListDto>>(payrollPeriods);
        }

        public async Task<PayrollPeriodDetailDto?> GetDetailAsync(int id)
        {
            var payrollPeriod = await _payrollPeriodRepository.GetDetailAsync(id);

            return payrollPeriod == null
                ? null
                : _mapper.Map<PayrollPeriodDetailDto>(payrollPeriod);
        }

        public async Task CreateAsync(PayrollPeriodCreateDto dto)
        {
            EnsureManagementAccess();

            if (dto.StartDate > dto.EndDate)
                throw new Exception("Başlangıç tarihi bitiş tarihinden büyük olamaz.");

            if (await _payrollPeriodRepository.ExistsAsync(dto.Year, dto.Month))
                throw new Exception("Bu bordro dönemi zaten mevcut.");

            var payrollPeriod = new PayrollPeriod
            {
                Year = dto.Year,
                Month = dto.Month,
                Name = new DateTime(dto.Year, dto.Month, 1)
                    .ToString("MMMM yyyy", new CultureInfo("tr-TR")),
                StartDate = dto.StartDate.Date,
                EndDate = dto.EndDate.Date,
                Status = PayrollPeriodStatus.Draft
            };

            await _payrollPeriodRepository.AddAsync(payrollPeriod);
            await _payrollPeriodRepository.SaveChangesAsync();

            await LogAsync(
                AuditAction.Created,
                payrollPeriod.Id,
                $"{payrollPeriod.Name} bordro dönemi oluşturuldu.");
        }

        public async Task GeneratePayrollsAsync(int id)
        {
            EnsureManagementAccess();

            var payrollPeriod = await _payrollPeriodRepository.GetByIdAsync(id);

            if (payrollPeriod == null)
                throw new Exception("Bordro dönemi bulunamadı.");

            if (payrollPeriod.Status != PayrollPeriodStatus.Draft)
                throw new Exception("Sadece taslak dönemlerde bordro oluşturulabilir.");

            var activeEmployees = await _employeePayrollRepository.GetActiveEmployeesAsync();

            foreach (var employee in activeEmployees)
            {
                var payrollExists = await _employeePayrollRepository
                    .ExistsAsync(id, employee.Id);

                if (payrollExists)
                    continue;

                var overtimeHours = await _employeePayrollRepository
                    .GetApprovedOvertimeHoursAsync(
                        employee.Id,
                        payrollPeriod.StartDate,
                        payrollPeriod.EndDate);

                var overtimeAmount = Round(
                    employee.Salary / PayrollConstants.MonthlyWorkingHours * overtimeHours);

                var payroll = new EmployeePayroll
                {
                    PayrollPeriodId = id,
                    EmployeeId = employee.Id,
                    BaseSalary = employee.Salary,
                    OvertimeHours = overtimeHours,
                    OvertimeAmount = overtimeAmount,
                    NetSalary = Round(employee.Salary + overtimeAmount),
                    Status = EmployeePayrollStatus.Draft
                };

                await _employeePayrollRepository.AddAsync(payroll);
            }

            await _employeePayrollRepository.SaveChangesAsync();

            await LogAsync(
                AuditAction.PayrollGenerated,
                id,
                $"{payrollPeriod.Name} dönemi için bordrolar oluşturuldu.");
        }

        public async Task ApproveAsync(int id)
        {
            EnsureAdminAccess();

            var payrollPeriod = await _payrollPeriodRepository.GetByIdAsync(id);

            if (payrollPeriod == null)
                throw new Exception("Bordro dönemi bulunamadı.");

            if (payrollPeriod.Status != PayrollPeriodStatus.Draft)
                throw new Exception("Sadece taslak dönem onaylanabilir.");

            var payrolls = await _employeePayrollRepository.GetByPeriodAsync(id);

            foreach (var payroll in payrolls)
            {
                var employeePayroll = await _employeePayrollRepository.GetByIdAsync(payroll.Id);

                if (employeePayroll == null)
                    continue;

                employeePayroll.Status = EmployeePayrollStatus.Approved;
                _employeePayrollRepository.Update(employeePayroll);
            }

            payrollPeriod.Status = PayrollPeriodStatus.Approved;

            _payrollPeriodRepository.Update(payrollPeriod);
            await _payrollPeriodRepository.SaveChangesAsync();

            await LogAsync(
                AuditAction.Approved,
                id,
                $"{payrollPeriod.Name} bordro dönemi onaylandı.");
        }

        public async Task RevertApprovalAsync(int id)
        {
            EnsureAdminAccess();

            var payrollPeriod = await _payrollPeriodRepository.GetByIdAsync(id);

            if (payrollPeriod == null)
                throw new Exception("Bordro dönemi bulunamadı.");

            if (payrollPeriod.Status != PayrollPeriodStatus.Approved)
                throw new Exception("Yalnızca onaylı dönemlerin onayı geri alınabilir.");

            var payrolls = await _employeePayrollRepository.GetByPeriodAsync(id);

            if (payrolls.Any(x => x.Status == EmployeePayrollStatus.Paid))
                throw new Exception("Ödenmiş bordrosu bulunan dönemin onayı geri alınamaz.");

            foreach (var payroll in payrolls)
            {
                var employeePayroll = await _employeePayrollRepository.GetByIdAsync(payroll.Id);

                if (employeePayroll == null)
                    continue;

                employeePayroll.Status = EmployeePayrollStatus.Draft;
                _employeePayrollRepository.Update(employeePayroll);
            }

            payrollPeriod.Status = PayrollPeriodStatus.Draft;

            _payrollPeriodRepository.Update(payrollPeriod);
            await _payrollPeriodRepository.SaveChangesAsync();

            await LogAsync(
                AuditAction.StatusChanged,
                id,
                $"{payrollPeriod.Name} bordro dönemi onayı geri alındı.");
        }

        public async Task MarkAsPaidAsync(int id)
        {
            EnsureAdminAccess();

            var payrollPeriod = await _payrollPeriodRepository.GetByIdAsync(id);

            if (payrollPeriod == null)
                throw new Exception("Bordro dönemi bulunamadı.");

            if (payrollPeriod.Status != PayrollPeriodStatus.Approved)
                throw new Exception("Sadece onaylı dönem ödenmiş yapılabilir.");

            var payrolls = await _employeePayrollRepository.GetByPeriodAsync(id);

            foreach (var payroll in payrolls)
            {
                var employeePayroll = await _employeePayrollRepository.GetByIdAsync(payroll.Id);

                if (employeePayroll == null)
                    continue;

                if (!employeePayroll.PaymentDate.HasValue)
                    employeePayroll.PaymentDate = DateTime.Today;

                employeePayroll.Status = EmployeePayrollStatus.Paid;
                _employeePayrollRepository.Update(employeePayroll);
            }

            payrollPeriod.Status = PayrollPeriodStatus.Paid;

            _payrollPeriodRepository.Update(payrollPeriod);
            await _payrollPeriodRepository.SaveChangesAsync();

            await LogAsync(
                AuditAction.MarkedAsPaid,
                id,
                $"{payrollPeriod.Name} bordro dönemi ödendi olarak işaretlendi.");
        }

        public async Task ChangeStatusAsync(int id, PayrollPeriodStatus status)
        {
            EnsureAdminAccess();

            if (!Enum.IsDefined(status))
                throw new Exception("Geçersiz bordro dönemi durumu.");

            var payrollPeriod = await _payrollPeriodRepository.GetByIdAsync(id);

            if (payrollPeriod == null)
                throw new Exception("Bordro dönemi bulunamadı.");

            var payrolls = await _employeePayrollRepository.GetByPeriodAsync(id);

            foreach (var payroll in payrolls)
            {
                var employeePayroll = await _employeePayrollRepository.GetByIdAsync(payroll.Id);

                if (employeePayroll == null)
                    continue;

                employeePayroll.Status = (EmployeePayrollStatus)status;
                _employeePayrollRepository.Update(employeePayroll);
            }

            payrollPeriod.Status = status;

            _payrollPeriodRepository.Update(payrollPeriod);
            await _payrollPeriodRepository.SaveChangesAsync();

            await LogAsync(
                AuditAction.StatusChanged,
                id,
                $"{payrollPeriod.Name} bordro dönemi durumu {GetStatusText(status)} olarak güncellendi.");
        }

        private Task LogAsync(AuditAction action, int entityId, string description)
        {
            return _auditLogService.LogAsync(new AuditLogCreateDto
            {
                Module = AuditModule.Payroll,
                Action = action,
                EntityId = entityId,
                Description = description
            });
        }

        private void EnsureManagementAccess()
        {
            if (!_currentUser.IsInRole(Roles.Admin) && !_currentUser.IsInRole(Roles.HR))
                throw new UnauthorizedAccessException("Bu işlem için yetkiniz bulunmuyor.");
        }

        private void EnsureAdminAccess()
        {
            if (!_currentUser.IsInRole(Roles.Admin))
                throw new UnauthorizedAccessException("Bu işlem için yalnızca Admin yetkilidir.");
        }

        private static string GetStatusText(PayrollPeriodStatus status)
        {
            return status switch
            {
                PayrollPeriodStatus.Draft => "Taslak",
                PayrollPeriodStatus.Approved => "Onaylandı",
                PayrollPeriodStatus.Paid => "Ödendi",
                _ => throw new ArgumentOutOfRangeException(nameof(status))
            };
        }

        private static decimal Round(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
