using HRFlow.Business.Interfaces;
using HRFlow.Business.Services;
using HRFlow.Web.Models.Department;
using HRFlow.Web.Models.Position;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _positionService.GetPositionListAsync();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new PositionCreateModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PositionCreateModel model)
        {
            await _positionService.CreateAsync(model.Position);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var position = await _positionService.GetByIdForUpdateAsync(id);

            if (position == null)
                return NotFound();

            var model = new PositionEditViewModel
            {
                Position = position
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PositionEditViewModel model)
        {
            await _positionService.UpdateAsync(model.Position);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _positionService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
