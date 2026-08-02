using HRFlow.Common.Interfaces;
using HRFlow.Entities.Enums;
using HRFlow.Entities.Identity;
using HRFlow.Entities.Logging;

namespace HRFlow.Data.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<List<Notification>> GetByUserAsync(string userId, bool? isRead);
        Task<List<Notification>> GetLatestByUserAsync(string userId, int count);
        Task<int> GetUnreadCountAsync(string userId);
        Task<string?> GetUserIdByEmployeeIdAsync(int employeeId);
        Task<List<SystemUser>> GetActiveNotificationRecipientsAsync();
        Task<HashSet<string>> GetExistingRecipientUserIdsAsync(
            AuditModule sourceModule,
            int sourceEntityId,
            NotificationEventType eventKey);
        Task<bool> ExistsAsync(string userId, AuditModule sourceModule, int sourceEntityId, NotificationEventType eventKey);
    }
}
