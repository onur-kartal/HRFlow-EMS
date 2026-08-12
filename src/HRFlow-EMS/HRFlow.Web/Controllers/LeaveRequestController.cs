using HRFlow.Business.DTOs.LeaveRequest;
using HRFlow.Business.Interfaces;
using HRFlow.Business.Services;
using HRFlow.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRFlow.Web.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILeaveTypeService _leaveTypeService;
        public LeaveRequestController(
                ILeaveRequestService leaveRequestService,
                ILeaveTypeService leaveTypeService)
        {
            _leaveRequestService = leaveRequestService;
            _leaveTypeService = leaveTypeService;
        }
        public async Task<IActionResult> Index()
        {
            var leaveRequests = await _leaveRequestService.GetLeaveRequestListAsync();

            return View(leaveRequests);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadLeaveTypesAsync();

            return View(new LeaveRequestCreateDto());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadLeaveTypesAsync();
                return View(dto);
            }

            try
            {
                await _leaveRequestService.CreateAsync(dto);

                TempData["Success"] = "İzin talebi başarıyla oluşturuldu.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await LoadLeaveTypesAsync();

                return View(dto);
            }
        }
        [HttpPost]
        [Authorize(Roles = Roles.Employee + "," + Roles.Manager + "," + Roles.HR + "," + Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _leaveRequestService.CancelAsync(id);

                TempData["Success"] = "İzin talebi başarıyla iptal edildi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _leaveRequestService.GetByIdForUpdateAsync(id);

                if (model == null)
                    return NotFound();

                await LoadLeaveTypesAsync();

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LeaveRequestUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadLeaveTypesAsync();
                return View(dto);
            }

            try
            {
                await _leaveRequestService.UpdateAsync(dto);

                TempData["Success"] = "İzin talebi başarıyla güncellendi.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await LoadLeaveTypesAsync();

                return View(dto);
            }
        }
        [HttpPost]
        [Authorize(Roles = Roles.Manager + "," + Roles.HR + "," + Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(LeaveRequestApproveDto dto)
        {
            try
            {
                await _leaveRequestService.ApproveAsync(dto);

                TempData["Success"] = "İzin talebi onaylandı.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Authorize(Roles = Roles.Manager + "," + Roles.HR + "," + Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                await _leaveRequestService.RejectAsync(id);

                TempData["Success"] = "İzin talebi reddedildi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        private async Task LoadLeaveTypesAsync()
        {
            var leaveTypes = await _leaveTypeService.GetLeaveTypeLookupAsync();

            ViewBag.LeaveTypes = leaveTypes
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });
        }
    }
}
