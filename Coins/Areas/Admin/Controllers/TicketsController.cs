using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Coins.Entities;
using Coins.Library;
using Coins.Models;
using Coins.Repositories;
using Coins.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coins.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TicketsController : Controller
    {
        public IAccountService _userService;
        public ITicketsService _ticketsService;
        public IEarningsService _earningsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketsController(
           IAccountService userService,
           ITicketsService ticketsService,
           IEarningsService earningsService,
           UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _userService = userService;
            _ticketsService = ticketsService;
            _earningsService = earningsService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Tickets = _ticketsService.GetAllTickets();

            return View();
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ResponseToTicket(TicketResponseViewModel model, int id)
        {
            await _ticketsService.ResponseToTicket(model, HttpContext, id);

            return RedirectToAction(nameof(Ticket), new { id = id });
        }

        public async Task<IActionResult> Ticket(int id)
        {
            ViewBag.User = await _userManager.GetUserAsync(HttpContext.User);
            //TODO ONLY FOR ADMIN
            ViewBag.Ticket = await _ticketsService.GetUserTicket(HttpContext, id);
            ViewBag.Responses = _ticketsService.GetResponsesToTicket(id);

            return View();
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            await _ticketsService.Deactivate(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
