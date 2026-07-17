using HRFlow.Business.DTOs.Department;
using HRFlow.Business.Interfaces;
using HRFlow.Business.Services;
using HRFlow.Web.Models.Department;
using HRFlow.Web.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace HRFlow.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _departmentService.GetDepartmentListAsync();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new DepartmentCreateModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _departmentService.CreateAsync(model.Department);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetByIdForUpdateAsync(id);

            if (department == null)
                return NotFound();

            var model = new DepartmentEditViewModel
            {
                Department = department
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentEditViewModel model)
        {
            await _departmentService.UpdateAsync(model.Department);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
