using HRFlow.Business.DTOs.OvertimeRequest;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    [Authorize]
    public class OvertimeRequestController : Controller
    {
        private readonly IOvertimeRequestService _overtimeRequestService;

        public OvertimeRequestController(IOvertimeRequestService overtimeRequestService)
        {
            _overtimeRequestService = overtimeRequestService;
        }

        public async Task<IActionResult> MyRequests()
        {
            var overtimeRequests = await _overtimeRequestService.GetMyRequestsAsync();

            return View(overtimeRequests);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new OvertimeRequestCreateDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OvertimeRequestCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _overtimeRequestService.CreateAsync(dto);
                TempData["Success"] = "Fazla mesai talebi başarıyla oluşturuldu.";

                return RedirectToAction(nameof(MyRequests));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [Authorize(Roles = Roles.Manager + "," + Roles.HR + "," + Roles.Admin)]
        public async Task<IActionResult> ApprovalList()
        {
            var overtimeRequests = await _overtimeRequestService.GetPendingRequestsAsync();

            return View(overtimeRequests);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Management()
        {
            var overtimeRequests = await _overtimeRequestService.GetAllRequestsAsync();

            return View(overtimeRequests);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Manager + "," + Roles.HR + "," + Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                await _overtimeRequestService.ApproveAsync(id);
                TempData["Success"] = "Fazla mesai talebi onaylandı.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(ApprovalList));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Manager + "," + Roles.HR + "," + Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                await _overtimeRequestService.RejectAsync(id);
                TempData["Success"] = "Fazla mesai talebi reddedildi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(ApprovalList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id, bool fromApprovalList = false)
        {
            try
            {
                await _overtimeRequestService.CancelAsync(id);
                TempData["Success"] = "Fazla mesai talebi iptal edildi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(fromApprovalList ? nameof(ApprovalList) : nameof(MyRequests));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(OvertimeRequestStatusChangeDto dto)
        {
            try
            {
                await _overtimeRequestService.ChangeStatusAsync(dto);
                TempData["Success"] = "Fazla mesai talebinin durumu güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Management));
        }
    }
}
