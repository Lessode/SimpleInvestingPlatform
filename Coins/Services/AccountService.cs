using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Coins.Repositories;
using Coins.Services.Interfaces;
using Coins.Entities;
using Coins.Library;
using Coins.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Coins.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<ApplicationUser> _userRepository;
        private Mapper<ApplicationUser, ApplicationUserViewModel> _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IGenericRepository<ApplicationUser> userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _mapper = new Mapper<ApplicationUser, ApplicationUserViewModel>();
        }

        public async Task<bool> Login(ApplicationUserViewModel viewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(
                viewModel.Email,
                viewModel.Password,
                true,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> Register(string id, ApplicationUserViewModel viewModel)
        {
            ApplicationUser user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email, Refferal = id };
            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                return true;
            }

            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetUserRefferals(HttpContext httpContext)
        {
            ApplicationUser user = await _userManager.GetUserAsync(httpContext.User);

            List<ApplicationUserViewModel> refferalViewModels = new List<ApplicationUserViewModel>();
            IEnumerable<ApplicationUser> refferals = _userManager.Users.Where(a => a.Refferal == user.Id);

            foreach (var item in refferals)
            {
                refferalViewModels.Add(_mapper.EntityToViewModel(new ApplicationUserViewModel(), item));
            }

            return refferalViewModels.AsEnumerable();
        }

        public async Task Edit(ClaimsPrincipal httpContextUser, ApplicationUserViewModel model)
        {
            ApplicationUser user = await _userManager.GetUserAsync(httpContextUser);

            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;

            await _userManager.UpdateAsync(user);
        }

        public async Task EditAdmin(ApplicationUserViewModel model, string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.Balance = model.Balance;

            await _userManager.UpdateAsync(user);
        }

        public async Task EditPassword(ClaimsPrincipal httpContextUser, string OldPassword, string Password)
        {
            ApplicationUser user = await _userManager.GetUserAsync(httpContextUser);

            await _userManager.ChangePasswordAsync(user, OldPassword, Password);
        }
    }
}
