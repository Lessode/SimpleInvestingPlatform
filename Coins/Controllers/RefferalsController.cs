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

namespace Coins.Controllers
{
    [Authorize]
    public class RefferalsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IAccountService _userService;

        public RefferalsController(
            IAccountService userService,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Refferals = await _userService.GetUserRefferals(HttpContext);
            return View();
        }
    }
}
