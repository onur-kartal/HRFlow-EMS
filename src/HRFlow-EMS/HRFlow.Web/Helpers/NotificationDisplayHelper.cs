using HRFlow.Entities.Enums;

namespace HRFlow.Web.Helpers
{
    public static class NotificationDisplayHelper
    {
        public static string GetIcon(NotificationType notificationType)
        {
            return notificationType switch
            {
                NotificationType.Leave => "bi-calendar-check",
                NotificationType.Overtime => "bi-clock-history",
                NotificationType.Announcement => "bi-megaphone",
                NotificationType.Payroll => "bi-cash-stack",
                NotificationType.Success => "bi-check-circle",
                NotificationType.Warning => "bi-exclamation-triangle",
                NotificationType.Error => "bi-x-circle",
                _ => "bi-info-circle"
            };
        }

        public static string GetTextClass(NotificationType notificationType)
        {
            return notificationType switch
            {
                NotificationType.Leave => "text-primary",
                NotificationType.Overtime => "text-warning",
                NotificationType.Announcement => "text-info",
                NotificationType.Payroll => "text-success",
                NotificationType.Error => "text-danger",
                _ => "text-secondary"
            };
        }
    }
}
