using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.ViewComponents
{
    public class DashboardSidebarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
