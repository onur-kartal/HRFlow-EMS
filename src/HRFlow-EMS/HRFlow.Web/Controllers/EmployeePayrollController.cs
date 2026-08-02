using HRFlow.Business.DTOs.Payroll;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Constants;
using HRFlow.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    [Authorize]
    public class EmployeePayrollController : Controller
    {
        private readonly IEmployeePayrollService _employeePayrollService;
        private readonly IPayrollPdfService _payrollPdfService;

        public EmployeePayrollController(
            IEmployeePayrollService employeePayrollService,
            IPayrollPdfService payrollPdfService)
        {
            _employeePayrollService = employeePayrollService;
            _payrollPdfService = payrollPdfService;
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
        public async Task<IActionResult> Index()
        {
            var payrolls = await _employeePayrollService.GetManagementListAsync();

            return View(payrolls);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
        public async Task<IActionResult> Details(int id)
        {
            var payroll = await _employeePayrollService.GetDetailAsync(id);

            return payroll == null
                ? NotFound()
                : View(payroll);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
        public async Task<IActionResult> Edit(int id)
        {
            var payroll = await _employeePayrollService.GetDetailAsync(id);

            if (payroll == null)
                return NotFound();

            var dto = new EmployeePayrollUpdateDto
            {
                Id = payroll.Id,
                Bonus = payroll.Bonus,
                Deduction = payroll.Deduction,
                PaymentDate = payroll.PaymentDate
            };

            return View(dto);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeePayrollUpdateDto dto)
        {
            try
            {
                await _employeePayrollService.UpdateAsync(dto);

                TempData["Success"] = "Bordro güncellendi.";

                return RedirectToAction(nameof(Details), new { id = dto.Id });
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);

                return View(dto);
            }
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            return await UpdateStatusAsync(
                id,
                _employeePayrollService.ApproveAsync,
                "Bordro onaylandı.");
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            return await UpdateStatusAsync(
                id,
                _employeePayrollService.MarkAsPaidAsync,
                "Bordro ödendi.");
        }

        public async Task<IActionResult> MyPayrolls()
        {
            var payrolls = await _employeePayrollService.GetMyPayrollsAsync();

            return View(payrolls);
        }

        public async Task<IActionResult> MyDetails(int id)
        {
            try
            {
                var payroll = await _employeePayrollService.GetMyDetailAsync(id);

                return View(payroll);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        public async Task<IActionResult> ViewPdf(int id)
        {
            return await GetPayrollPdfAsync(id, false);
        }

        public async Task<IActionResult> DownloadPdf(int id)
        {
            return await GetPayrollPdfAsync(id, true);
        }

        private async Task<IActionResult> UpdateStatusAsync(
            int id,
            Func<int, Task> updateStatus,
            string successMessage)
        {
            try
            {
                await updateStatus(id);

                TempData["Success"] = successMessage;
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        private async Task<IActionResult> GetPayrollPdfAsync(int id, bool download)
        {
            try
            {
                var payroll = await _employeePayrollService.GetMyDetailAsync(id);

                if (payroll == null)
                    return NotFound();

                var content = _payrollPdfService.Generate(payroll);

                if (download)
                {
                    var fileName = $"Bordro-{payroll.PeriodName}-{payroll.Id}.pdf";

                    return File(content, "application/pdf", fileName);
                }

                return File(content, "application/pdf");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}
