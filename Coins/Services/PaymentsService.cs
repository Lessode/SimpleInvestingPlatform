using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Coins.Entities;
using Coins.Repositories;
using Coins.Services.Interfaces;
using Coins.Models;
using Coins.Library;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using CoinbaseConnector;
using System.Globalization;
using System.Net.Http;

namespace Coins.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly IGenericRepository<PutMoney> _putMoneyRepository;
        private Mapper<Payment, PaymentViewModel> _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private Mapper<PutMoney, PutMoneyViewModel> _putMoneyMapper;
        private Mapper<Payment, PaymentViewModel> _widthdrawsMapper;
        private ISettingsRepository _settingsRepository;

        public PaymentsService(
            IGenericRepository<Payment> paymentRepository,
            IGenericRepository<PutMoney> putMoneyRepository,
            UserManager<ApplicationUser> userManager,
            ISettingsRepository settingsRepository)
        {
            _paymentRepository = paymentRepository;
            _putMoneyRepository = putMoneyRepository;
            _mapper = new Mapper<Payment, PaymentViewModel>();
            _putMoneyMapper = new Mapper<PutMoney, PutMoneyViewModel>();
            _widthdrawsMapper = new Mapper<Payment, PaymentViewModel>();
            _userManager = userManager;
            _settingsRepository = settingsRepository;
        }

        public async Task Withdraw(HttpContext httpContext, PaymentViewModel model)
        {
            var user = await _userManager.GetUserAsync(httpContext.User);

            if (user.Balance >= model.Money)
            {
                var connector = new Connector();

                JsonConvert.DeserializeObject<SendMoney_Result>(connector.SendMoney(model.Email, model.Money.ToString()));
            }
        }

        public async Task<string> PutMoney(HttpContext httpContext, PutMoneyViewModel model)
        {
            var user = await _userManager.GetUserAsync(httpContext.User);
            PutMoney putMoney = _putMoneyMapper.ViewModelToEntity(new PutMoney(), model);

            putMoney.Used = false;
            putMoney.UserId = user.Id;
            putMoney.Date = DateTime.Now;
            dynamic result = await GetJsonDataFromUrl();

            putMoney.Email = result.input_address;

            _putMoneyRepository.Insert(putMoney);
            await _putMoneyRepository.Save();

            return result.input_address;
        }

        private async Task<dynamic> GetJsonDataFromUrl()
        {
            Setting settings = _settingsRepository.GetSettings();
            string secret = "zyrafywchodzodoszafy";
            string my_address = settings.WalletAddress;
            string my_callback_url = "http://79.137.37.203/deposit/callback?invoice_id=1234&secret=" + secret;
            string api_base = "https://blockchainapi.org/api/receive";
            HttpClient httpClient = new HttpClient();


            var url = api_base + "?method=create&address=" + my_address + "&callback=" + System.Net.WebUtility.HtmlEncode(my_callback_url);

            var result = await httpClient.GetStringAsync(url);

            return StringToJson(result);
        }

        private dynamic StringToJson(string data)
        {
            return JsonConvert.DeserializeObject(data);
        }

        public async Task UpdateUserData(ApplicationUser user, decimal amount)
        {
            user.Deposit = user.Deposit + amount;

            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateTransaction(PutMoney putMoney, decimal amount)
        {
            putMoney.Used = true;
            putMoney.Money = amount;

            _putMoneyRepository.Update(putMoney);
            await _putMoneyRepository.Save();
        }

        public async Task<IEnumerable<PutMoneyViewModel>> getUsersPayments(HttpContext httpContext)
        {
            ApplicationUser user = await _userManager.GetUserAsync(httpContext.User);
            List<PutMoneyViewModel> putMoneyViewModels = new List<PutMoneyViewModel>();
            IEnumerable<PutMoney> putMoneys = _putMoneyRepository.GetAll().Where(a => a.UserId == user.Id && a.Used == true);

            foreach (var item in putMoneys)
            {
                putMoneyViewModels.Add(_putMoneyMapper.EntityToViewModel(new PutMoneyViewModel(), item));
            }

            return putMoneyViewModels.AsEnumerable();
        }

        public decimal GetAllMoneyFromDeposits()
        {
            //TODO: OPTIMIZE QUERY WITH INDIVIDUAL REPOSITORY
            return _putMoneyRepository.GetAll().Where(a => a.Used == true).Sum(a => a.Money);
        }

        public async Task<IEnumerable<PaymentViewModel>> GetWithdraws(HttpContext httpContext)
        {
            ApplicationUser user = await _userManager.GetUserAsync(httpContext.User);
            List<PaymentViewModel> withdrawsViewModels = new List<PaymentViewModel>();
            IEnumerable<Payment> withdraws = _paymentRepository.GetAll().Where(a => a.UserId == user.Id);

            foreach (var item in withdraws)
            {
                withdrawsViewModels.Add(_widthdrawsMapper.EntityToViewModel(new PaymentViewModel(), item));
            }

            return withdrawsViewModels.AsEnumerable();
        }
    }
}
