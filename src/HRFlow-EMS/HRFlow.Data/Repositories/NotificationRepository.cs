using HRFlow.Data.Context;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.Enums;
using HRFlow.Entities.Identity;
using HRFlow.Entities.Logging;
using Microsoft.EntityFrameworkCore;

namespace HRFlow.Data.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(HRFlowDbContext context)
            : base(context)
        {
        }

        public async Task<List<Notification>> GetByUserAsync(string userId, bool? isRead)
        {
            var query = _context.Notifications
                .Where(x => !x.IsDeleted && x.UserId == userId);

            if (isRead.HasValue)
            {
                query = query.Where(x => x.IsRead == isRead.Value);
            }

            return await query
                .OrderByDescending(x => x.CreatedDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Notification>> GetLatestByUserAsync(string userId, int count)
        {
            return await _context.Notifications
                .Where(x => !x.IsDeleted && x.UserId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .Take(count)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<int> GetUnreadCountAsync(string userId)
        {
            return _context.Notifications.CountAsync(x => !x.IsDeleted && x.UserId == userId && !x.IsRead);
        }

        public Task<string?> GetUserIdByEmployeeIdAsync(int employeeId)
        {
            return _context.Users
                .Where(x => x.EmployeeId == employeeId && !x.Employee.IsDeleted && x.Employee.IsActive)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<SystemUser>> GetActiveNotificationRecipientsAsync()
        {
            return await _context.Users
                .Include(x => x.Employee)
                .Where(x => x.Employee == null || (!x.Employee.IsDeleted && x.Employee.IsActive))
                .ToListAsync();
        }

        public async Task<HashSet<string>> GetExistingRecipientUserIdsAsync(
            AuditModule sourceModule,
            int sourceEntityId,
            NotificationEventType eventKey)
        {
            return await _context.Notifications
                .Where(x => !x.IsDeleted &&
                            x.SourceModule == sourceModule &&
                            x.SourceEntityId == sourceEntityId &&
                            x.EventKey == eventKey)
                .Select(x => x.UserId)
                .ToHashSetAsync();
        }

        public Task<bool> ExistsAsync(string userId, AuditModule sourceModule, int sourceEntityId, NotificationEventType eventKey)
        {
            return _context.Notifications.AnyAsync(x =>
                !x.IsDeleted &&
                x.UserId == userId &&
                x.SourceModule == sourceModule &&
                x.SourceEntityId == sourceEntityId &&
                x.EventKey == eventKey);
        }
    }
}
