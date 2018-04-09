using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Coins.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Coins.Entities;
using Coins.Models;
using Coins.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coins.Controllers
{
    [Authorize]
    public class DepositController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IPaymentsService _paymentsService;
        private IGenericRepository<PutMoney> _putMoneyRepository;
        private ISettingsRepository _settingsRepository;

        public DepositController(
            UserManager<ApplicationUser> userManager,
            IPaymentsService paymentsService,
            IGenericRepository<PutMoney> putMoneyRepository,
            ISettingsRepository settingsRepository)
        {
            _userManager = userManager;
            _paymentsService = paymentsService;
            _settingsRepository = settingsRepository;
            _putMoneyRepository = putMoneyRepository;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.UserPayments = await _paymentsService.getUsersPayments(HttpContext);
            ;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Put(PutMoneyViewModel model)
        {
            string btc = await _paymentsService.PutMoney(HttpContext, model);
            return RedirectToAction(nameof(Status), new { btc = btc });
        }

        public async Task<IActionResult> Status(string btc)
        {
            ViewBag.User = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Settings = _settingsRepository.GetSettings();
            ViewBag.Btc = btc;

            return View();
        }

        public async Task Callback(string secret, string input_address, string value)
        {
            if (secret == "zyrafywchodzodoszafy")
            {
                //TODO: OPTIMIZE
                PutMoney putMoney = _putMoneyRepository.GetAll().Where(a => a.Email == input_address).Last();
                ApplicationUser user = await _userManager.FindByIdAsync(putMoney.UserId);

                // Update transaction - set as used
                await _paymentsService.UpdateTransaction(putMoney, decimal.Parse(value, CultureInfo.InvariantCulture));

                // Add lead and update money
                await _paymentsService.UpdateUserData(user, decimal.Parse(value, CultureInfo.InvariantCulture));
            }
        }
    }
}
