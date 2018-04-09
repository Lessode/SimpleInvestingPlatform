using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Coins.Entities;
using Coins.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coins.Controllers
{
    [Authorize]
    public class EarningsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IEarningsService _earningsService;
        private IPaymentsService _paymentsService;


        public EarningsController(
            UserManager<ApplicationUser> userManager,
            IPaymentsService paymentsService,

            IEarningsService earningsService)
        {
            _userManager = userManager;
            _earningsService = earningsService;
            _paymentsService = paymentsService;

        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Earnings = await _earningsService.getUserEarnings(HttpContext);

            return View();
        }
    }
}
