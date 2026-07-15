using AutoMapper;
using HRFlow.Business.DTOs.Dashboard;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.Interfaces;
using HRFlow.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IMapper _mapper;

        public DashboardService(
            IDepartmentRepository departmentRepository,
            IPositionRepository positionRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _mapper = mapper;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            // 1. Son eklenen personelleri getir
            var lastEmployees = await _employeeRepository.GetLastEmployeesAsync(5);

            // 2. Sayıları getir
            var employeeCount = await _employeeRepository.GetEmployeeCountAsync();
            var departmentCount = await _departmentRepository.GetDepartmentCountAsync();
            var positionCount = await _positionRepository.GetPositionCountAsync();

            var employees = await _employeeRepository.GetEmployeesWithDepartmentAsync();

            var departmentChart = employees
                .GroupBy(x => x.Department.Name)
                .Select(x => new DepartmentChartDto
                {
                    DepartmentName = x.Key,
                    EmployeeCount = x.Count()
                })
                .OrderByDescending(x => x.EmployeeCount)
                .ToList();

            // 3. DTO oluştur ve View'a gönder
            return new DashboardDto
            {
                EmployeeCount = employeeCount,
                DepartmentCount = departmentCount,
                PositionCount = positionCount,
                LeaveCount = 0,

                LastEmployees = _mapper.Map<List<EmployeeListDto>>(lastEmployees),
                DepartmentChart = departmentChart
            };
        }
    }
}
