using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Coins.Entities;
using Coins.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coins.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public UserManager<ApplicationUser> _userManager;
        IPaymentsService _paymentsService;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IPaymentsService paymentsService)
        {
            _userManager = userManager;
            _paymentsService = paymentsService;
        }
        
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Users = _userManager.Users.Count();
            ViewBag.Deposits = _paymentsService.GetAllMoneyFromDeposits();

            return View();
        }
    }
}
