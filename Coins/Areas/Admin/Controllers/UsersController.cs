using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Coins.Entities;
using Coins.Services.Interfaces;
using Coins.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coins.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public UsersController(
            UserManager<ApplicationUser> userManager,
            IAccountService accountService,
             SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _accountService = accountService;
            _signInManager = signInManager;
        }
        // GET: /<controller>/
        public async Task <IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.Users = _userManager.Users.ToList();
            return View();
        }
   
        public async Task<IActionResult> EditUser(string id)
        {
            ViewBag.User = await _userManager.FindByIdAsync(id);

            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> EditUser(ApplicationUserViewModel model, string id)
        {
            await _accountService.EditAdmin(model, id);

            return RedirectToAction(nameof(EditUser), new { id = id });
        }

        public async Task<IActionResult> BlockUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SetAdmin(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            await _userManager.AddToRoleAsync(user, "Admin");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveAdmin(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            await _userManager.RemoveFromRoleAsync(user, "Admin");

            return RedirectToAction(nameof(Index));
        }
    }
}
