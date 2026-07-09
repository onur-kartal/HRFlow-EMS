using HRFlow.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _employeeService.GetEmployeeListAsync();

            return View(model);
        }
    }
}
