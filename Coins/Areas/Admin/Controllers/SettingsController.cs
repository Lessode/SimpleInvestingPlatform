using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Coins.Repositories;
using Microsoft.AspNetCore.Identity;
using Coins.Entities;
using Coins.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coins.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        public ISettingsRepository _settingsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public SettingsController(
           ISettingsRepository settingsRepository,
           UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _settingsRepository = settingsRepository;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Settings = _settingsRepository.GetSettings();
            return View();
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(SettingViewModel model)
        {
            Setting settings = _settingsRepository.GetSettings();

            settings.WalletAddress = model.WalletAddress;

            _settingsRepository.Update(settings);
            await _settingsRepository.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
