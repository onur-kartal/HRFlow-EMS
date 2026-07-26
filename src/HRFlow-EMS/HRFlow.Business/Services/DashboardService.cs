using AutoMapper;
using HRFlow.Business.DTOs.Dashboard;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.DTOs.LeaveRequest;
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
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IAnnouncementService _announcementService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public DashboardService(
            IDepartmentRepository departmentRepository,
            IPositionRepository positionRepository,
            IEmployeeRepository employeeRepository,
            ILeaveRequestRepository leaveRequestRepository,
            IAnnouncementService announcementService,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _announcementService = announcementService;
            _currentUser = currentUser;
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
                DepartmentChart = departmentChart,

                TodayOnLeaveCount = await _leaveRequestRepository.GetTodayOnLeaveCountAsync(),
                PendingLeaveRequests = _mapper.Map<List<PendingLeaveDto>>(await _leaveRequestRepository.GetPendingLeaveRequestsAsync(5)),
                UpcomingLeaveRequests = _mapper.Map<List<UpcomingLeaveDto>>(await _leaveRequestRepository.GetUpcomingLeaveRequestsAsync(5)),
                Announcements = await _announcementService.GetActiveDashboardAnnouncementsAsync(5)
            };
        }

        public async Task<ManagerDashboardDto> GetManagerDashboardAsync()
        {
            return new ManagerDashboardDto
            {
                PendingLeaveCount = await _leaveRequestRepository.GetLeaveRequestCountByStatusAsync(HRFlow.Common.Enums.LeaveStatus.Pending),
                ApprovedLeaveCount = await _leaveRequestRepository.GetLeaveRequestCountByStatusAsync(HRFlow.Common.Enums.LeaveStatus.Approved),
                RejectedLeaveCount = await _leaveRequestRepository.GetLeaveRequestCountByStatusAsync(HRFlow.Common.Enums.LeaveStatus.Rejected),
                CancelledLeaveCount = await _leaveRequestRepository.GetLeaveRequestCountByStatusAsync(HRFlow.Common.Enums.LeaveStatus.Cancelled),
                Announcements = await _announcementService.GetActiveDashboardAnnouncementsAsync(5)
            };
        }

        public async Task<EmployeeDashboardDto> GetEmployeeDashboardAsync()
        {
            var employeeId = _currentUser.EmployeeId;

            return new EmployeeDashboardDto
            {
                PendingLeaveCount = await _leaveRequestRepository.GetLeaveRequestCountByStatusAsync(HRFlow.Common.Enums.LeaveStatus.Pending, employeeId),
                ApprovedLeaveCount = await _leaveRequestRepository.GetLeaveRequestCountByStatusAsync(HRFlow.Common.Enums.LeaveStatus.Approved, employeeId),
                RejectedLeaveCount = await _leaveRequestRepository.GetLeaveRequestCountByStatusAsync(HRFlow.Common.Enums.LeaveStatus.Rejected, employeeId),
                CancelledLeaveCount = await _leaveRequestRepository.GetLeaveRequestCountByStatusAsync(HRFlow.Common.Enums.LeaveStatus.Cancelled, employeeId),
                Announcements = await _announcementService.GetActiveDashboardAnnouncementsAsync(5)
            };
        }
    }
}
