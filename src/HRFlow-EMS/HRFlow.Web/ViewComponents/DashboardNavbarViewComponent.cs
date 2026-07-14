using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.ViewComponents
{
    public class DashboardNavbarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
