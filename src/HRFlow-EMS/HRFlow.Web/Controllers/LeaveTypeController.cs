using HRFlow.Business.Interfaces;
using HRFlow.Business.Services;
using HRFlow.Common.Constants;
using HRFlow.Web.Models.Department;
using HRFlow.Web.Models.LeaveType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveTypeController(ILeaveTypeService leaveTypeService)
        {
            _leaveTypeService = leaveTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _leaveTypeService.GetleaveTypeListAsync();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new LeaveTypeCreateModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(LeaveTypeCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _leaveTypeService.CreateAsync(model.LeaveType);

            TempData["Success"] = "İzin türü başarıyla oluşturuldu.";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var leaveType = await _leaveTypeService.GetByIdForUpdateAsync(id);

            if (leaveType == null)
                return NotFound();

            var model = new LeaveTypeEditViewModel
            {
                LeaveType = leaveType
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(LeaveTypeEditViewModel model)
        {
            await _leaveTypeService.UpdateAsync(model.LeaveType);

            TempData["Success"] = "İzin türü başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _leaveTypeService.DeleteAsync(id);

            TempData["Success"] = "İzin türü başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }
    }
}
