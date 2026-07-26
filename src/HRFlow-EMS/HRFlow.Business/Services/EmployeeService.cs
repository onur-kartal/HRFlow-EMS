using AutoMapper;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Interfaces;
using HRFlow.Data.Repositories;
using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Organization;
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

        public EmployeeService(
         IGenericRepository<Employee> repository,
         IEmployeeRepository employeeRepository,
         IDepartmentRepository departmentRepository,
         IPositionRepository positionRepository,
         IAccountService accountService,
         IMapper mapper)
         : base(repository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _accountService = accountService;
        }

        public async Task CreateAsync(EmployeeCreateDto dto)
        {
            var employee = _mapper.Map<Employee>(dto);

            await _repository.AddAsync(employee);

            await _repository.SaveChangesAsync();
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
            var employee = await _repository.GetByIdAsync(dto.Id);

            if (employee == null)
                return;

            _mapper.Map(dto, employee);

            _repository.Update(employee);

            await _repository.SaveChangesAsync();
        }

        public async Task<List<EmployeeLookupDto>> GetEmployeeLookupAsync()
        {
            var employees = await _employeeRepository.GetEmployeeListAsync();

            return _mapper.Map<List<EmployeeLookupDto>>(employees);
        }
    }
}
