using HRFlow.Entities.Enums;

namespace HRFlow.Business.DTOs.Notification
{
    public class NotificationListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType NotificationType { get; set; }
        public string? Url { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public AuditModule? SourceModule { get; set; }
        public int? SourceEntityId { get; set; }
        public NotificationEventType? EventKey { get; set; }
    }
}
