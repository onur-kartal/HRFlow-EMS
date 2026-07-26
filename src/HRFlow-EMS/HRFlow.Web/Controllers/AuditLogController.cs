using HRFlow.Business.Interfaces;
using HRFlow.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AuditLogController : Controller
    {
        private readonly IAuditLogService _service;
        public AuditLogController(IAuditLogService service) => _service = service;
        public async Task<IActionResult> Index() => View(await _service.GetListAsync());
    }
}
