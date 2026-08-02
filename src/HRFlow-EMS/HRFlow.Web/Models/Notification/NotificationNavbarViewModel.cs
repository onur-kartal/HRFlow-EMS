using HRFlow.Business.DTOs.Account;
using HRFlow.Business.DTOs.Notification;

namespace HRFlow.Web.Models.Notification
{
    public class NotificationNavbarViewModel
    {
        public ProfileDto? Profile { get; set; }
        public NotificationNavbarDto NotificationSummary { get; set; } = new();
    }
}
