using HRFlow.Business.DTOs.Payroll;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Constants;
using HRFlow.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
    public class PayrollPeriodController : Controller
    {
        private readonly IPayrollPeriodService _payrollPeriodService;

        public PayrollPeriodController(IPayrollPeriodService payrollPeriodService)
        {
            _payrollPeriodService = payrollPeriodService;
        }

        public async Task<IActionResult> Index()
        {
            var payrollPeriods = await _payrollPeriodService.GetListAsync();

            return View(payrollPeriods);
        }

        public IActionResult Create()
        {
            var previousMonth = DateTime.Today.AddMonths(-1);

            return View(new PayrollPeriodCreateDto
            {
                Year = previousMonth.Year,
                Month = previousMonth.Month,
                StartDate = new DateTime(previousMonth.Year, previousMonth.Month, 1),
                EndDate = new DateTime(
                    previousMonth.Year,
                    previousMonth.Month,
                    DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month))
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PayrollPeriodCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _payrollPeriodService.CreateAsync(dto);

                TempData["Success"] = "Bordro dönemi oluşturuldu.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);

                return View(dto);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var payrollPeriod = await _payrollPeriodService.GetDetailAsync(id);

            return payrollPeriod == null
                ? NotFound()
                : View(payrollPeriod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GeneratePayrolls(int id)
        {
            return await ExecutePeriodActionAsync(
                id,
                _payrollPeriodService.GeneratePayrollsAsync,
                "Bordrolar oluşturuldu.");
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            return await ExecutePeriodActionAsync(
                id,
                _payrollPeriodService.ApproveAsync,
                "Dönem onaylandı.");
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RevertApproval(int id)
        {
            return await ExecutePeriodActionAsync(
                id,
                _payrollPeriodService.RevertApprovalAsync,
                "Dönem onayı geri alındı.");
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            return await ExecutePeriodActionAsync(
                id,
                _payrollPeriodService.MarkAsPaidAsync,
                "Dönem ödendi olarak işaretlendi.");
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, PayrollPeriodStatus status)
        {
            try
            {
                await _payrollPeriodService.ChangeStatusAsync(id, status);

                TempData["Success"] = "Dönem durumu güncellendi.";
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        private async Task<IActionResult> ExecutePeriodActionAsync(
            int id,
            Func<int, Task> periodAction,
            string successMessage)
        {
            try
            {
                await periodAction(id);

                TempData["Success"] = successMessage;
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
