using Coins.Entities;
using Coins.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coins.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Login(ApplicationUserViewModel viewModel);
        Task<bool> Register(string id, ApplicationUserViewModel viewModel);
        Task Logout();
        Task<IEnumerable<ApplicationUserViewModel>> GetUserRefferals(HttpContext httpContext);
        Task Edit(ClaimsPrincipal httpContextUser, ApplicationUserViewModel entity);
        Task EditAdmin(ApplicationUserViewModel entity, string id);
        Task EditPassword(ClaimsPrincipal httpContextUser, string OldPassword, string Password);
    }
}
