using AutoMapper;
using HRFlow.Business.DTOs.Notification;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Interfaces;
using HRFlow.Data.Interfaces;
using HRFlow.Entities.Enums;
using HRFlow.Entities.Logging;

namespace HRFlow.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public NotificationService(
            INotificationRepository notificationRepository,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<List<NotificationListDto>> GetMyNotificationsAsync(bool? isRead)
        {
            var userId = GetCurrentUserId();
            var notifications = await _notificationRepository.GetByUserAsync(userId, isRead);

            return _mapper.Map<List<NotificationListDto>>(notifications);
        }

        public async Task<NotificationNavbarDto> GetNavbarNotificationsAsync()
        {
            var userId = GetCurrentUserId();
            var notifications = await _notificationRepository.GetLatestByUserAsync(userId, 5);

            return new NotificationNavbarDto
            {
                UnreadCount = await _notificationRepository.GetUnreadCountAsync(userId),
                Notifications = _mapper.Map<List<NotificationListDto>>(notifications)
            };
        }

        public async Task<NotificationOpenDto?> OpenAsync(int id)
        {
            var notification = await GetOwnedNotificationAsync(id);

            if (notification.IsRead)
            {
                return new NotificationOpenDto { Url = notification.Url };
            }

            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            _notificationRepository.Update(notification);
            await _notificationRepository.SaveChangesAsync();

            return new NotificationOpenDto { Url = notification.Url };
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await GetOwnedNotificationAsync(id);

            if (notification.IsRead)
            {
                return;
            }

            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            _notificationRepository.Update(notification);
            await _notificationRepository.SaveChangesAsync();
        }

        public async Task MarkAllAsReadAsync()
        {
            var userId = GetCurrentUserId();
            var unreadNotifications = await _notificationRepository.GetByUserAsync(userId, false);

            if (unreadNotifications.Count == 0)
            {
                return;
            }

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.ReadDate = DateTime.UtcNow;
                _notificationRepository.Update(notification);
            }

            await _notificationRepository.SaveChangesAsync();
        }

        public async Task CreateForEmployeeAsync(int employeeId, string title, string message, NotificationType notificationType, string? url, AuditModule sourceModule, int sourceEntityId, NotificationEventType eventKey)
        {
            var userId = await _notificationRepository.GetUserIdByEmployeeIdAsync(employeeId);

            if (string.IsNullOrWhiteSpace(userId) || await _notificationRepository.ExistsAsync(userId, sourceModule, sourceEntityId, eventKey))
            {
                return;
            }

            await CreateAsync(new NotificationCreateDto
            {
                UserId = userId,
                Title = title,
                Message = message,
                NotificationType = notificationType,
                Url = url,
                SourceModule = sourceModule,
                SourceEntityId = sourceEntityId,
                EventKey = eventKey
            });
        }

        public async Task CreateForActiveUsersAsync(string title, string message, NotificationType notificationType, string? url, AuditModule sourceModule, int sourceEntityId, NotificationEventType eventKey)
        {
            var recipients = await _notificationRepository.GetActiveNotificationRecipientsAsync();
            var existingUserIds = await _notificationRepository.GetExistingRecipientUserIdsAsync(sourceModule, sourceEntityId, eventKey);
            var hasNewNotification = false;

            foreach (var recipient in recipients)
            {
                if (existingUserIds.Contains(recipient.Id))
                {
                    continue;
                }

                await _notificationRepository.AddAsync(_mapper.Map<Notification>(new NotificationCreateDto
                {
                    UserId = recipient.Id,
                    Title = title,
                    Message = message,
                    NotificationType = notificationType,
                    Url = url,
                    SourceModule = sourceModule,
                    SourceEntityId = sourceEntityId,
                    EventKey = eventKey
                }));
                hasNewNotification = true;
            }

            if (hasNewNotification)
            {
                await _notificationRepository.SaveChangesAsync();
            }
        }

        private async Task CreateAsync(NotificationCreateDto dto)
        {
            await _notificationRepository.AddAsync(_mapper.Map<Notification>(dto));
            await _notificationRepository.SaveChangesAsync();
        }

        private async Task<Notification> GetOwnedNotificationAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);

            if (notification == null)
            {
                throw new KeyNotFoundException("Bildirim bulunamadı.");
            }

            if (notification.UserId != GetCurrentUserId())
            {
                throw new UnauthorizedAccessException("Bu bildirim için yetkiniz bulunmuyor.");
            }

            return notification;
        }

        private string GetCurrentUserId()
        {
            return _currentUser.UserId
                ?? throw new UnauthorizedAccessException("Giriş yapan kullanıcı bulunamadı.");
        }
    }
}
