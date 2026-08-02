using AutoMapper;
using HRFlow.Business.DTOs.Logging;
using HRFlow.Business.DTOs.Payroll;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Constants;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.Enums;

namespace HRFlow.Business.Services
{
    public class EmployeePayrollService : IEmployeePayrollService
    {
        private readonly IEmployeePayrollRepository _employeePayrollRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IAuditLogService _auditLogService;
        private readonly IMapper _mapper;

        public EmployeePayrollService(
            IEmployeePayrollRepository employeePayrollRepository,
            ICurrentUserService currentUser,
            IAuditLogService auditLogService,
            IMapper mapper)
        {
            _employeePayrollRepository = employeePayrollRepository;
            _currentUser = currentUser;
            _auditLogService = auditLogService;
            _mapper = mapper;
        }

        public async Task<List<EmployeePayrollListDto>> GetManagementListAsync()
        {
            EnsureManagementAccess();

            var payrolls = await _employeePayrollRepository.GetManagementListAsync();

            return _mapper.Map<List<EmployeePayrollListDto>>(payrolls);
        }

        public async Task<EmployeePayrollDetailDto?> GetDetailAsync(int id)
        {
            EnsureManagementAccess();

            var payroll = await _employeePayrollRepository.GetDetailAsync(id);

            return payroll == null
                ? null
                : _mapper.Map<EmployeePayrollDetailDto>(payroll);
        }

        public async Task UpdateAsync(EmployeePayrollUpdateDto dto)
        {
            EnsureManagementAccess();

            if (dto.Bonus < 0 || dto.Deduction < 0)
                throw new Exception("Prim ve kesinti negatif olamaz.");

            var payroll = await _employeePayrollRepository.GetByIdAsync(dto.Id);

            if (payroll == null)
                throw new Exception("Bordro bulunamadı.");

            if (payroll.Status != EmployeePayrollStatus.Draft)
                throw new Exception("Sadece taslak bordrolar düzenlenebilir.");

            payroll.Bonus = dto.Bonus;
            payroll.Deduction = dto.Deduction;
            payroll.PaymentDate = dto.PaymentDate;
            payroll.NetSalary = Round(
                payroll.BaseSalary +
                payroll.OvertimeAmount +
                payroll.Bonus -
                payroll.Deduction);

            if (payroll.NetSalary < 0)
                throw new Exception("Net maaş negatif olamaz.");

            _employeePayrollRepository.Update(payroll);
            await _employeePayrollRepository.SaveChangesAsync();

            await LogAsync(
                AuditAction.Updated,
                payroll.Id,
                "Çalışan bordrosu güncellendi.");
        }

        public async Task ApproveAsync(int id)
        {
            EnsureManagementAccess();

            var payroll = await _employeePayrollRepository.GetByIdAsync(id);

            if (payroll == null)
                throw new Exception("Bordro bulunamadı.");

            if (payroll.Status != EmployeePayrollStatus.Draft)
                throw new Exception("Sadece taslak bordrolar onaylanabilir.");

            payroll.Status = EmployeePayrollStatus.Approved;

            _employeePayrollRepository.Update(payroll);
            await _employeePayrollRepository.SaveChangesAsync();

            await LogAsync(
                AuditAction.Approved,
                payroll.Id,
                "Çalışan bordrosu onaylandı.");
        }

        public async Task MarkAsPaidAsync(int id)
        {
            EnsureManagementAccess();

            var payroll = await _employeePayrollRepository.GetByIdAsync(id);

            if (payroll == null)
                throw new Exception("Bordro bulunamadı.");

            if (payroll.Status != EmployeePayrollStatus.Approved || !payroll.PaymentDate.HasValue)
                throw new Exception("Onaylı bordro ve ödeme tarihi gereklidir.");

            payroll.Status = EmployeePayrollStatus.Paid;

            _employeePayrollRepository.Update(payroll);
            await _employeePayrollRepository.SaveChangesAsync();

            await LogAsync(
                AuditAction.MarkedAsPaid,
                payroll.Id,
                "Çalışan bordrosu ödendi olarak işaretlendi.");
        }

        public async Task<List<MyPayrollListDto>> GetMyPayrollsAsync()
        {
            if (_currentUser.EmployeeId <= 0)
                throw new UnauthorizedAccessException();

            var payrolls = await _employeePayrollRepository
                .GetByEmployeeAsync(_currentUser.EmployeeId);

            if (!CanViewUnpaidPayrolls())
            {
                payrolls = payrolls
                    .Where(x => x.Status == EmployeePayrollStatus.Paid)
                    .ToList();
            }

            return _mapper.Map<List<MyPayrollListDto>>(payrolls);
        }

        public async Task<EmployeePayrollDetailDto?> GetMyDetailAsync(int id)
        {
            var payroll = await _employeePayrollRepository.GetDetailAsync(id);

            if (payroll == null)
                throw new UnauthorizedAccessException("Bu bordroya erişim yetkiniz bulunmuyor.");

            if (!CanViewUnpaidPayrolls() &&
                (payroll.EmployeeId != _currentUser.EmployeeId ||
                 payroll.Status != EmployeePayrollStatus.Paid))
            throw new UnauthorizedAccessException("Bu bordroya erişim yetkiniz bulunmuyor.");

            return _mapper.Map<EmployeePayrollDetailDto>(payroll);
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

        private bool CanViewUnpaidPayrolls()
        {
            return _currentUser.IsInRole(Roles.Admin) ||
                   _currentUser.IsInRole(Roles.HR);
        }

        private static decimal Round(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
