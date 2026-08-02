using HRFlow.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool? isRead)
        {
            var notifications = await _notificationService.GetMyNotificationsAsync(isRead);
            ViewBag.IsRead = isRead;

            return View(notifications);
        }

        [HttpGet]
        public async Task<IActionResult> Open(int id)
        {
            try
            {
                var notification = await _notificationService.OpenAsync(id);

                if (!string.IsNullOrWhiteSpace(notification?.Url) && Url.IsLocalUrl(notification.Url))
                {
                    return LocalRedirect(notification.Url);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id, bool? isRead)
        {
            try
            {
                await _notificationService.MarkAsReadAsync(id);
                TempData["Success"] = "Bildirim okundu olarak işaretlendi.";
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index), new { isRead });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllAsRead(bool? isRead)
        {
            await _notificationService.MarkAllAsReadAsync();
            TempData["Success"] = "Tüm bildirimler okundu olarak işaretlendi.";

            return RedirectToAction(nameof(Index), new { isRead });
        }
    }
}
