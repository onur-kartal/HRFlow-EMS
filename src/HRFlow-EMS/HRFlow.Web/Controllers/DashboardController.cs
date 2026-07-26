using HRFlow.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly ICurrentUserService _currentUser;

        public DashboardController(IDashboardService dashboardService, ICurrentUserService currentUser)
        {
            _dashboardService = dashboardService;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dashboardService.GetDashboardAsync();
            Console.WriteLine(_currentUser.EmployeeId);

            Console.WriteLine(_currentUser.Email);

            Console.WriteLine(_currentUser.UserName);
            return View(model);
        }
    }
}
