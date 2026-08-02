namespace HRFlow.Business.DTOs.Notification
{
    public class NotificationNavbarDto
    {
        public int UnreadCount { get; set; }
        public List<NotificationListDto> Notifications { get; set; } = [];
    }
}
