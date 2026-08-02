using AutoMapper;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Interfaces;
using HRFlow.Data.Repositories;
using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Organization;
using HRFlow.Business.DTOs.Logging;
using HRFlow.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Services
{
    public class EmployeeService : GenericService<Employee>, IEmployeeService
    {

        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IAccountService _accountService;
        private readonly IAuditLogService _auditLogService;

        public EmployeeService(
         IGenericRepository<Employee> repository,
         IEmployeeRepository employeeRepository,
         IDepartmentRepository departmentRepository,
         IPositionRepository positionRepository,
         IAccountService accountService, IAuditLogService auditLogService,
         IMapper mapper)
         : base(repository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _accountService = accountService;
            _auditLogService = auditLogService;
        }

        public async Task CreateAsync(EmployeeCreateDto dto)
        {
            EnsureBirthDateIsValid(dto.BirthDate);
            var employee = _mapper.Map<Employee>(dto);
            employee.IsActive = true;

            await _repository.AddAsync(employee);

            await _repository.SaveChangesAsync();
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module=AuditModule.Employee, Action=AuditAction.Created, EntityId=employee.Id, Description=$"{employee.FirstName} {employee.LastName} isimli çalışan oluşturuldu." });
        }

        public async Task<List<EmployeeListDto>> GetEmployeeListAsync()
        {
            var employees = await _employeeRepository.GetEmployeeListAsync();

            var employeeList = _mapper.Map<List<EmployeeListDto>>(employees);

            foreach (var employee in employeeList)
            {
                if (employee.HasUser)
                {
                    employee.UserRole = await _accountService.GetUserRoleAsync(employee.Id);
                }
            }

            return employeeList;
        }
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _departmentRepository.GetDepartmentListAsync();
        }

        public async Task<List<Position>> GetPositionsAsync()
        {
            return await _positionRepository.GetPositionListAsync();
        }

        public async Task<EmployeeUpdateDto?> GetByIdForUpdateAsync(int id)
        {
            var employee = await _repository.GetByIdAsync(id);

            if (employee == null)
                return null;

            return _mapper.Map<EmployeeUpdateDto>(employee);
        }

        public async Task UpdateAsync(EmployeeUpdateDto dto)
        {
            EnsureBirthDateIsValid(dto.BirthDate);
            var employee = await _repository.GetByIdAsync(dto.Id);

            if (employee == null)
                return;

            _mapper.Map(dto, employee);

            _repository.Update(employee);

            await _repository.SaveChangesAsync();
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module=AuditModule.Employee, Action=AuditAction.Updated, EntityId=employee.Id, Description=$"{employee.FirstName} {employee.LastName} isimli çalışan güncellendi." });
        }

        public async Task<List<EmployeeLookupDto>> GetEmployeeLookupAsync()
        {
            var employees = await _employeeRepository.GetEmployeeListAsync();

            return _mapper.Map<List<EmployeeLookupDto>>(employees);
        }

        public async Task<EmployeeDetailDto?> GetEmployeeDetailAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeDetailAsync(id);
            return employee == null ? null : _mapper.Map<EmployeeDetailDto>(employee);
        }

        public async Task<List<UpcomingBirthdayDto>> GetUpcomingBirthdaysAsync()
        {
            var employees = await _employeeRepository.GetEmployeesWithBirthDateAsync();
            var birthdays = _mapper.Map<List<UpcomingBirthdayDto>>(employees);
            var today = DateTime.Today;

            foreach (var birthday in birthdays)
            {
                birthday.DaysLeft = GetDaysUntilBirthday(birthday.BirthDate, today);
            }

            return birthdays
                .Where(x => x.DaysLeft >= 0 && x.DaysLeft <= 5)
                .OrderBy(x => x.DaysLeft)
                .ThenBy(x => x.FullName)
                .ToList();
        }

        private static void EnsureBirthDateIsValid(DateTime? birthDate)
        {
            if (birthDate.HasValue && birthDate.Value.Date > DateTime.Today)
                throw new ArgumentException("Doğum tarihi gelecekte olamaz.");
        }

        private static int GetDaysUntilBirthday(DateTime birthDate, DateTime today)
        {
            var birthdayThisYear = GetBirthdayDate(birthDate, today.Year);
            var nextBirthday = birthdayThisYear < today
                ? GetBirthdayDate(birthDate, today.Year + 1)
                : birthdayThisYear;

            return (nextBirthday - today).Days;
        }

        private static DateTime GetBirthdayDate(DateTime birthDate, int year)
        {
            if (birthDate.Month == 2 && birthDate.Day == 29 && !DateTime.IsLeapYear(year))
                return new DateTime(year, 2, 28);

            return new DateTime(year, birthDate.Month, birthDate.Day);
        }
        public override async Task DeleteAsync(int id)
        {
            var employee = await _repository.GetByIdAsync(id); if (employee == null) return;
            _repository.Delete(employee); await _repository.SaveChangesAsync();
            await _auditLogService.LogAsync(new AuditLogCreateDto { Module=AuditModule.Employee, Action=AuditAction.Deleted, EntityId=id, Description=$"{employee.FirstName} {employee.LastName} isimli çalışan silindi." });
        }
    }
}
