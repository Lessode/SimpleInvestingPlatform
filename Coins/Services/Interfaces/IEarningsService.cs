using Coins.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coins.Services.Interfaces
{
    public interface IEarningsService
    {
        Task AppendBonus();
        Task<IEnumerable<EarningViewModel>> getUserEarnings(HttpContext httpContext);
        Task<decimal> countUserEarnings(HttpContext httpContext);
        decimal countUsersEarnings();
    }
}
