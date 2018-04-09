using Coins.Entities;
using Coins.Library;
using Coins.Models;
using Coins.Repositories;
using Coins.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coins.Services
{
    public class EarningsService : IEarningsService
    {
        IGenericRepository<Earning> _earningsRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<PutMoney> _putMoneyRepository;
        private Mapper<Earning, EarningViewModel> _earningsMapper;


        public EarningsService(
            IGenericRepository<Earning> earningsRepository,
            IGenericRepository<PutMoney> putMoneyRepository,
            UserManager<ApplicationUser> userManager)
        {
            _earningsRepository = earningsRepository;
            _putMoneyRepository = putMoneyRepository;
            _userManager = userManager;
            _earningsMapper = new Mapper<Earning, EarningViewModel>();
        }

        public async Task AppendBonus()
        {
            while (true)
            {
                await DailyBonus();
                await MonthlyBonus();

                await Task.Delay(TimeSpan.FromDays(1));
            }
        }

        private async Task DailyBonus()
        {
            // TODO: OPTIMIZE - REMOVE 'GETALL' AND CREATE FILTER IN REPOSITORY
            var putMoneys = _putMoneyRepository
                .GetAll()
                .Where(a =>
                    a.Used == true
                    && a.Active != true
                );

            foreach (var item in putMoneys)
            {
                decimal moneyToAppend = Decimal.Multiply(item.Money, new Decimal(2.2)) / 100;

                await UpdateUserBalance(item.UserId, moneyToAppend);

                await CreateStatistics(moneyToAppend, item.UserId);
            }
        }

        private async Task CreateStatistics(decimal moneyToAppend, string userId)
        {
            Earning earning = new Earning
            {
                Date = DateTime.Now,
                Money = moneyToAppend,
                UserId = userId
            };

            _earningsRepository.Insert(earning);
            await _earningsRepository.Save();
        }

        private async Task MonthlyBonus()
        {
            // TODO: OPTIMIZE - REMOVE 'GETALL' AND CREATE FILTER IN REPOSITORY
            var putMoneys = _putMoneyRepository
                .GetAll()
                .Where(a =>
                    a.Used == true
                    && a.Date.AddDays(25).Date == DateTime.Now.Date
                    || a.Date.AddDays(30).Date == DateTime.Now.Date
                );

            foreach (var item in putMoneys)
            {
                decimal moneyToAppend = Decimal.Multiply(item.Money, new Decimal(50)) / 100;

                // Remove deposit after 25 days or 30 days
                await UpdateUserData(item.User.Id, moneyToAppend);
                await DeactivateItem(item);
            }

        }

        private async Task UpdateUserBalance(string userId, decimal moneyToAppend)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            user.Balance += moneyToAppend;

            await _userManager.UpdateAsync(user);

            await UpdateRefferalBalance(user.Refferal, moneyToAppend);
        }

        private async Task UpdateRefferalBalance(string refferal, decimal moneyToAppend)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(refferal);
            user.Balance += Decimal.Multiply(moneyToAppend, new Decimal(20)) / 100;

            await _userManager.UpdateAsync(user);
        }

        private async Task UpdateUserData(string userId, decimal moneyToAppend)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            user.Balance += moneyToAppend;
            user.Deposit -= moneyToAppend;

            await _userManager.UpdateAsync(user);
        }

        private async Task DeactivateItem(PutMoney item)
        {
            if(item.Date.AddDays(30).Date == DateTime.Now.Date)
            {
                item.Active = true;

                _putMoneyRepository.Update(item);
                await _putMoneyRepository.Save();
            }
        }

        public async Task<IEnumerable<EarningViewModel>> getUserEarnings(HttpContext httpContext)
        {
            ApplicationUser user = await _userManager.GetUserAsync(httpContext.User);
            List<EarningViewModel> earningViewModels = new List<EarningViewModel>();
            IEnumerable<Earning> earnings = _earningsRepository.GetAll().Where(a => a.UserId == user.Id);

            foreach (var item in earnings)
            {
                earningViewModels.Add(_earningsMapper.EntityToViewModel(new EarningViewModel(), item));
            }

            return earningViewModels.AsEnumerable();
        }

        public async Task<decimal> countUserEarnings(HttpContext httpContext)
        {
            //TODO: Optimize
            ApplicationUser user = await _userManager.GetUserAsync(httpContext.User);

            return _earningsRepository.GetAll().Where(a => a.UserId == user.Id).Sum(a => a.Money);
        }

        public decimal countUsersEarnings()
        {
            return _earningsRepository.GetAll().Sum(a => a.Money);
        }
    }
}
