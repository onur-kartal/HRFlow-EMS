using HRFlow.Entities.Base;
using HRFlow.Entities.Enums;
using HRFlow.Entities.Identity;

namespace HRFlow.Entities.Logging
{
    public class Notification : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        public SystemUser User { get; set; } = null!;

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public NotificationType NotificationType { get; set; }

        public string? Url { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadDate { get; set; }

        public AuditModule? SourceModule { get; set; }

        public int? SourceEntityId { get; set; }

        public NotificationEventType? EventKey { get; set; }
    }
}
