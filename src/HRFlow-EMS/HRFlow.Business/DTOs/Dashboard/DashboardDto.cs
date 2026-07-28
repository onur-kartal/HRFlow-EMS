using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.DTOs.LeaveRequest;
using HRFlow.Business.DTOs.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Business.DTOs.Dashboard
{
    public class DashboardDto
    {
        public int EmployeeCount { get; set; }
        public int DepartmentCount { get; set; }
        public int PositionCount { get; set; }
        public int LeaveCount { get; set; }
        public List<EmployeeListDto> LastEmployees { get; set; } = [];
        public List<DepartmentChartDto> DepartmentChart { get; set; } = [];
        public int TodayOnLeaveCount { get; set; }
        public List<PendingLeaveDto> PendingLeaveRequests { get; set; } = new();
        public List<UpcomingLeaveDto> UpcomingLeaveRequests { get; set; } = new();
        public List<AnnouncementDashboardDto> Announcements { get; set; } = new();
        public List<UpcomingBirthdayDto> UpcomingBirthdays { get; set; } = new();
    }
}

