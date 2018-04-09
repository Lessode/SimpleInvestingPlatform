using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Coins.Services.Interfaces;
using Coins.Entities;
using Coins.Models;

namespace Coins.Controllers
{
    public class AccountController : Controller
    {
        public IAccountService _userService;
        public ITicketsService _ticketsService;
        public IEarningsService _earningsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private IPaymentsService _paymentsService;


        public AccountController(
            IAccountService userService,
            IPaymentsService paymentsService,
            ITicketsService ticketsService,
            IEarningsService earningsService,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _paymentsService = paymentsService;
            _userService = userService;
            _ticketsService = ticketsService;
            _earningsService = earningsService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var earnings = await _earningsService.countUserEarnings(HttpContext);
            ViewBag.User = user;
            ViewBag.Earnings = earnings;


            return View();
        }

        public IActionResult AccessDenied()
        {
            return RedirectToAction(nameof(Index), "Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            ViewBag.User = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Tickets = await _ticketsService.GetUserTickets(HttpContext);
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Ticket(int id)
        {
            ViewBag.User = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Ticket = await _ticketsService.GetUserTicket(HttpContext, id);
            ViewBag.Responses = _ticketsService.GetResponsesToTicket(id);

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

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> NewTicket(TicketViewModel ticket)
        {
            await _ticketsService.NewTicket(ticket, HttpContext);

            return RedirectToAction(nameof(Profile));
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeData(ApplicationUserViewModel model)
        {
            await _userService.Edit(HttpContext.User, model);

            return RedirectToAction(nameof(Profile));
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string OldPassword, string Password)
        {
            await _userService.EditPassword(HttpContext.User, OldPassword, Password);

            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(ApplicationUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(viewModel);

                if(result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string id, ApplicationUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(id, viewModel);

                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
