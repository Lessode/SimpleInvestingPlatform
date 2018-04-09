using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Coins.Services.Interfaces;
using Coins.Repositories;
using Coins.Entities;
using Coins.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coins.Controllers
{
    [Authorize]
    public class WithdrawController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IPaymentsService _paymentsService;
        private ISettingsRepository _settingsRepository;

        public WithdrawController(
            UserManager<ApplicationUser> userManager,
            IPaymentsService paymentsService,
            ISettingsRepository settingsRepository)
        {
            _userManager = userManager;
            _paymentsService = paymentsService;
            _settingsRepository = settingsRepository;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.GetWithdraws = await _paymentsService.GetWithdraws(HttpContext);
            return View();
        }

        [HttpPost]
        public IActionResult Put(PaymentViewModel model)
        {
            _paymentsService.Withdraw(HttpContext, model);

            return RedirectToAction(nameof(Index));
        }
    }
}
