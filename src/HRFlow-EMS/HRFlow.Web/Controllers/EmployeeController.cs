using HRFlow.Business.DTOs.Account;
using HRFlow.Business.DTOs.Employee;
using HRFlow.Business.Interfaces;
using HRFlow.Business.Services;
using HRFlow.Common.Constants;
using HRFlow.Web.Models.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRFlow.Web.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAccountService _accountService;

        public EmployeeController(IEmployeeService employeeService, IAccountService accountService)
        {
            _employeeService = employeeService;
            _accountService= accountService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _employeeService.GetEmployeeListAsync();

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new EmployeeCreateViewModel();

            model.Departments = (await _employeeService.GetDepartmentsAsync())
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            model.Positions = (await _employeeService.GetPositionsAsync())
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = (await _employeeService.GetDepartmentsAsync())
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }).ToList();

                model.Positions = (await _employeeService.GetPositionsAsync())
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }).ToList();

                return View(model);
            }

            await _employeeService.CreateAsync(model.Employee);

            TempData["Success"] = "Personel başarıyla oluşturuldu.";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetByIdForUpdateAsync(id);

            if (employee == null)
                return NotFound();

            var model = new EmployeeEditViewModel
            {
                Employee = employee
            };

            model.Departments = (await _employeeService.GetDepartmentsAsync())
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            model.Positions = (await _employeeService.GetPositionsAsync())
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = (await _employeeService.GetDepartmentsAsync())
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }).ToList();

                model.Positions = (await _employeeService.GetPositionsAsync())
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }).ToList();

                return View(model);
            }

            await _employeeService.UpdateAsync(model.Employee);

            TempData["Success"] = "Personel başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);

            TempData["Success"] = "Personel başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<ActionResult> CreateUser(int id)
        {
            var result = await _accountService.CreateUserFromEmployeeAsync(id);

            if (result)
            {
                TempData["Success"] = "Kullanıcı başarıyla oluşturuldu.";
            }
            else
            {
                TempData["Error"] = "Kullanıcı oluşturulamadı.";
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ChangeRole(int id)
        {
            var model = await _accountService.GetChangeRoleDtoAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(ChangeRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                var dto = await _accountService.GetChangeRoleDtoAsync(model.EmployeeId);

                return View(dto);
            }

            var result = await _accountService.ChangeRoleAsync(model);

            if (!result)
            {
                TempData["Error"] = "Rol güncellenemedi.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Rol başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }
    }
}
