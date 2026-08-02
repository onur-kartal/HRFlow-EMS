using HRFlow.Business.Interfaces;
using HRFlow.Web.Models.Notification;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.ViewComponents
{
    public class DashboardNavbarViewComponent : ViewComponent
    {
        private readonly IAccountService _accountService;
        private readonly INotificationService _notificationService;

        public DashboardNavbarViewComponent(
            IAccountService accountService,
            INotificationService notificationService)
        {
            _accountService = accountService;
            _notificationService = notificationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new NotificationNavbarViewModel
            {
                Profile = await _accountService.GetProfileAsync(),
                NotificationSummary = await _notificationService.GetNavbarNotificationsAsync()
            });
        }
    }
}
