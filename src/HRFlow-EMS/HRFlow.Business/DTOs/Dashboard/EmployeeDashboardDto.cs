using HRFlow.Business.DTOs.Announcement;

namespace HRFlow.Business.DTOs.Dashboard
{
    public class EmployeeDashboardDto
    {
        public int PendingLeaveCount { get; set; }
        public int ApprovedLeaveCount { get; set; }
        public int RejectedLeaveCount { get; set; }
        public int CancelledLeaveCount { get; set; }
        public List<AnnouncementDashboardDto> Announcements { get; set; } = new();
    }
}
