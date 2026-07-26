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
            var model = new HRFlow.Web.Models.Dashboard.DashboardViewModel();

            if (User.IsInRole(HRFlow.Common.Constants.Roles.Employee))
            {
                model.EmployeeDashboard = await _dashboardService.GetEmployeeDashboardAsync();
            }
            else if (User.IsInRole(HRFlow.Common.Constants.Roles.Manager))
            {
                model.ManagerDashboard = await _dashboardService.GetManagerDashboardAsync();
            }
            else
            {
                model.AdminHrDashboard = await _dashboardService.GetDashboardAsync();
            }

            return View(model);
        }
    }
}
