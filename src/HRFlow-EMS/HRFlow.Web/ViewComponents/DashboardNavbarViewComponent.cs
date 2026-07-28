using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.ViewComponents
{
    public class DashboardNavbarViewComponent : ViewComponent
    {
        private readonly HRFlow.Business.Interfaces.IAccountService _accountService;

        public DashboardNavbarViewComponent(HRFlow.Business.Interfaces.IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _accountService.GetProfileAsync());
        }
    }
}
