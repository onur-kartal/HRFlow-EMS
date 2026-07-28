using HRFlow.Business.DTOs.Account;
using HRFlow.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRFlow.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.LoginAsync(model);

            if (!result)
            {
                ModelState.AddModelError("", "E-posta adresi veya şifre hatalı.");
                return View(model);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var profile = await _accountService.GetProfileAsync();

            if (profile == null)
                return NotFound();

            return View(new HRFlow.Web.Models.Account.ProfileViewModel
            {
                Profile = profile,
                ProfileUpdate = new ProfileUpdateDto
                {
                    PhoneNumber = profile.PhoneNumber,
                    PersonalEmail = profile.PersonalEmail,
                    Address = profile.Address,
                    City = profile.City,
                    District = profile.District,
                    PostalCode = profile.PostalCode
                }
            });
        }

        [Authorize(Roles = HRFlow.Common.Constants.Roles.Admin + "," + HRFlow.Common.Constants.Roles.HR)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile([Bind(Prefix = "ProfileUpdate")] ProfileUpdateDto model)
        {
            var profile = await _accountService.GetProfileAsync();
            if (profile == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View("Profile", new HRFlow.Web.Models.Account.ProfileViewModel { Profile = profile, ProfileUpdate = model });

            if (!await _accountService.UpdateProfileAsync(model))
                return NotFound();

            TempData["Success"] = "Profil bilgileriniz başarıyla güncellendi.";
            return RedirectToAction(nameof(Profile));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(HRFlow.Web.Models.Account.ProfileViewModel model)
        {
            var profile = await _accountService.GetProfileAsync();

            if (profile == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.Profile = profile;
                return View("Profile", model);
            }

            var result = await _accountService.ChangePasswordAsync(model.ChangePassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                model.Profile = profile;
                return View("Profile", model);
            }

            TempData["Success"] = "Şifreniz başarıyla değiştirildi.";

            return RedirectToAction(nameof(Profile));
        }
    }
}
