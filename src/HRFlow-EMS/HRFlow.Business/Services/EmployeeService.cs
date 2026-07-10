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
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Position> _positionRepository;

        public EmployeeService(
         IGenericRepository<Employee> repository,
         IEmployeeRepository employeeRepository,
         IGenericRepository<Department> departmentRepository,
         IGenericRepository<Position> positionRepository,
         IMapper mapper)
         : base(repository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
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

            return _mapper.Map<List<EmployeeListDto>>(employees);
        }
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<List<Position>> GetPositionsAsync()
        {
            return await _positionRepository.GetAllAsync();
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
    }
}
