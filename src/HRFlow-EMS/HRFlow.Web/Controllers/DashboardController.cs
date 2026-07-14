using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
