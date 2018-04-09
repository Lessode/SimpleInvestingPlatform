using Coins.Entities;
using Coins.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coins.Services.Interfaces
{
    public interface IPaymentsService
    {
        Task Withdraw(HttpContext httpContext, PaymentViewModel model);
        Task<IEnumerable<PaymentViewModel>> GetWithdraws(HttpContext httpContext);
        Task<string> PutMoney(HttpContext httpContext, PutMoneyViewModel model);
        Task CheckMoney(HttpContext httpContext);
        Task<IEnumerable<PutMoneyViewModel>> getUsersPayments(HttpContext httpContext);
        decimal GetAllMoneyFromDeposits();
        Task UpdateUserData(ApplicationUser user, decimal amount);
        Task UpdateTransaction(PutMoney putMoney, decimal amount);
    }
}
