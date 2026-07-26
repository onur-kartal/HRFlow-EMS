using HRFlow.Business.DTOs.Announcement;
using HRFlow.Business.Interfaces;
using HRFlow.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public async Task<IActionResult> Index()
        {
            var announcements = await _announcementService.GetAnnouncementListAsync();

            return View(announcements);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AnnouncementCreateDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnouncementCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _announcementService.CreateAsync(dto);
                TempData["Success"] = "Duyuru başarıyla oluşturuldu.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var announcement = await _announcementService.GetByIdForUpdateAsync(id);

            return announcement == null ? NotFound() : View(announcement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AnnouncementUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _announcementService.UpdateAsync(dto);
                TempData["Success"] = "Duyuru başarıyla güncellendi.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _announcementService.DeleteAnnouncementAsync(id);
                TempData["Success"] = "Duyuru başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            try
            {
                await _announcementService.ChangeStatusAsync(id);
                TempData["Success"] = "Duyuru durumu güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
