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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(IEmployeeService employeeService, IAccountService accountService, IWebHostEnvironment webHostEnvironment)
        {
            _employeeService = employeeService;
            _accountService= accountService;
            _webHostEnvironment = webHostEnvironment;
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
            await ValidateAndSaveProfileImageAsync(model.ProfileImage, path => model.Employee.ProfileImagePath = path);

            if (!ModelState.IsValid)
            {
                await PopulateListsAsync(model);

                return View(model);
            }

            try
            {
                await _employeeService.CreateAsync(model.Employee);
            }
            catch (ArgumentException exception)
            {
                ModelState.AddModelError(nameof(model.Employee.BirthDate), exception.Message);
                await PopulateListsAsync(model);
                return View(model);
            }

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
            var currentEmployee = await _employeeService.GetByIdForUpdateAsync(model.Employee.Id);
            if (currentEmployee == null)
                return NotFound();

            await ValidateAndSaveProfileImageAsync(model.ProfileImage, path => model.Employee.ProfileImagePath = path);

            if (!ModelState.IsValid)
            {
                await PopulateListsAsync(model);

                return View(model);
            }

            try
            {
                await _employeeService.UpdateAsync(model.Employee);
                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                    DeleteOldProfileImage(currentEmployee.ProfileImagePath);
            }
            catch (ArgumentException exception)
            {
                ModelState.AddModelError(nameof(model.Employee.BirthDate), exception.Message);
                await PopulateListsAsync(model);
                return View(model);
            }

            TempData["Success"] = "Personel başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _employeeService.GetEmployeeDetailAsync(id);
            return model == null ? NotFound() : View(model);
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

        private async Task PopulateListsAsync(EmployeeCreateViewModel model)
        {
            model.Departments = (await _employeeService.GetDepartmentsAsync()).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            model.Positions = (await _employeeService.GetPositionsAsync()).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        private async Task PopulateListsAsync(EmployeeEditViewModel model)
        {
            model.Departments = (await _employeeService.GetDepartmentsAsync()).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            model.Positions = (await _employeeService.GetPositionsAsync()).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        private async Task ValidateAndSaveProfileImageAsync(IFormFile? profileImage, Action<string> setImagePath)
        {
            if (profileImage == null || profileImage.Length == 0)
                return;

            const long maximumFileSize = 2 * 1024 * 1024;
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(profileImage.FileName).ToLowerInvariant();

            if (profileImage.Length > maximumFileSize || !allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("ProfileImage", "Profil fotoğrafı jpg, jpeg, png veya webp formatında ve en fazla 2 MB olmalıdır.");
                return;
            }

            var directory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "employees");
            Directory.CreateDirectory(directory);
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(directory, fileName);

            await using var stream = System.IO.File.Create(filePath);
            await profileImage.CopyToAsync(stream);
            setImagePath($"/uploads/employees/{fileName}");

        }

        private void DeleteOldProfileImage(string? existingImagePath)
        {
            const string uploadPrefix = "/uploads/employees/";
            if (string.IsNullOrWhiteSpace(existingImagePath) || !existingImagePath.StartsWith(uploadPrefix, StringComparison.OrdinalIgnoreCase))
                return;

            var fileName = Path.GetFileName(existingImagePath);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "employees", fileName);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}
