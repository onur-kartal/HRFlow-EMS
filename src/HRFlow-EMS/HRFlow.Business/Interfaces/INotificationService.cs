using HRFlow.Business.DTOs.Notification;
using HRFlow.Entities.Enums;

namespace HRFlow.Business.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationListDto>> GetMyNotificationsAsync(bool? isRead);
        Task<NotificationNavbarDto> GetNavbarNotificationsAsync();
        Task<NotificationOpenDto?> OpenAsync(int id);
        Task MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync();
        Task CreateForEmployeeAsync(int employeeId, string title, string message, NotificationType notificationType, string? url, AuditModule sourceModule, int sourceEntityId, NotificationEventType eventKey);
        Task CreateForActiveUsersAsync(string title, string message, NotificationType notificationType, string? url, AuditModule sourceModule, int sourceEntityId, NotificationEventType eventKey);
    }
}
