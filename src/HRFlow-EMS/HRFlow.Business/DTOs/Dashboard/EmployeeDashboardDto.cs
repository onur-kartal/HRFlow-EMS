using HRFlow.Business.DTOs.Announcement;
using HRFlow.Business.DTOs.Employee;

namespace HRFlow.Business.DTOs.Dashboard
{
    public class EmployeeDashboardDto
    {
        public int PendingLeaveCount { get; set; }
        public int ApprovedLeaveCount { get; set; }
        public int RejectedLeaveCount { get; set; }
        public int CancelledLeaveCount { get; set; }
        public List<AnnouncementDashboardDto> Announcements { get; set; } = new();
        public List<UpcomingBirthdayDto> UpcomingBirthdays { get; set; } = new();
    }
}
