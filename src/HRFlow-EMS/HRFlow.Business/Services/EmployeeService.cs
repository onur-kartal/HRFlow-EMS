using AutoMapper;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.HumanResources;
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

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeListDto>> GetEmployeeListAsync()
        {
            var employees = await _employeeRepository.GetEmployeeListAsync();

            return _mapper.Map<List<EmployeeListDto>>(employees);
        }
    }
}
