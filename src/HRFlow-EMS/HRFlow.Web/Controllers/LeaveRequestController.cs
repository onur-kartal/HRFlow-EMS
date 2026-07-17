using HRFlow.Business.DTOs.LeaveRequest;
using HRFlow.Business.Interfaces;
using HRFlow.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRFlow.Web.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveTypeService _leaveTypeService;
        public LeaveRequestController(
                ILeaveRequestService leaveRequestService,
                IEmployeeService employeeService,
                ILeaveTypeService leaveTypeService)
        {
            _leaveRequestService = leaveRequestService;
            _employeeService = employeeService;
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
            await LoadLookupsAsync();

            return View(new LeaveRequestCreateDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequestCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync();
                return View(dto);
            }

            try
            {
                await _leaveRequestService.CreateAsync(dto);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await LoadLookupsAsync();

                return View(dto);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _leaveRequestService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _leaveRequestService.GetByIdForUpdateAsync(id);

            if (model == null)
                return NotFound();

            await LoadLookupsAsync();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(LeaveRequestUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync();
                return View(dto);
            }

            try
            {
                await _leaveRequestService.UpdateAsync(dto);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await LoadLookupsAsync();

                return View(dto);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                await _leaveRequestService.ApproveAsync(id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                await _leaveRequestService.RejectAsync(id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        private async Task LoadLookupsAsync()
        {
            var employees = await _employeeService.GetEmployeeLookupAsync();

            ViewBag.Employees = employees
                .OrderBy(x => x.FullName)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FullName
                });

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
