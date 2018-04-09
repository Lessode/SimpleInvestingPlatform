using Coins.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coins.Models;
using Microsoft.AspNetCore.Http;
using Coins.Entities;
using Coins.Library;
using Microsoft.AspNetCore.Identity;
using Coins.Repositories;

namespace Coins.Services
{
    public class TicketsService : ITicketsService
    {
        private readonly Mapper<Ticket, TicketViewModel> _mapper;
        private readonly Mapper<TicketResponse, TicketResponseViewModel> _mapperTicketsResponse;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Ticket> _ticketsRepository;
        private readonly IGenericRepository<TicketResponse> _ticketsResponseRepository;

        public TicketsService(
            UserManager<ApplicationUser> userManager,
            IGenericRepository<Ticket> ticketsRepository,
            IGenericRepository<TicketResponse> ticketsResponseRepository)
        {
            _userManager = userManager;
            _mapper = new Mapper<Ticket, TicketViewModel>();
            _mapperTicketsResponse = new Mapper<TicketResponse, TicketResponseViewModel>();
            _ticketsRepository = ticketsRepository;
            _ticketsResponseRepository = ticketsResponseRepository;
        }

        public async Task Deactivate(int ticketId)
        {
            Ticket ticket = _ticketsRepository.GetById(ticketId);

            ticket.Active = false;

            _ticketsRepository.Update(ticket);
            await _ticketsRepository.Save();
        }

        public async Task<TicketViewModel> GetUserTicket(HttpContext httpContext, int id)
        {
            Ticket ticket = _ticketsRepository.GetById(id);
            ApplicationUser user = await _userManager.GetUserAsync(httpContext.User);

            if (ticket.UserId == user.Id)
            {
                return _mapper.EntityToViewModel(new TicketViewModel(), ticket);
            }

            return null;
        }

        public async Task<IEnumerable<TicketViewModel>> GetUserTickets(HttpContext httpContext)
        {
            ApplicationUser user = await _userManager.GetUserAsync(httpContext.User);
            List<TicketViewModel> ticketViewModels = new List<TicketViewModel>();
            IEnumerable<Ticket> tickets = _ticketsRepository.GetAll().Where(a => a.UserId == user.Id);

            foreach (var item in tickets)
            {
                ticketViewModels.Add(_mapper.EntityToViewModel(new TicketViewModel(), item));
            }

            return ticketViewModels.AsEnumerable();
        }

        public IEnumerable<TicketViewModel> GetAllTickets()
        {
            List<TicketViewModel> ticketViewModels = new List<TicketViewModel>();
            IEnumerable<Ticket> tickets = _ticketsRepository.GetAll().Where(a => a.Active != false);

            foreach (var item in tickets)
            {
                ticketViewModels.Add(_mapper.EntityToViewModel(new TicketViewModel(), item));
            }

            return ticketViewModels.AsEnumerable();
        }

        public async Task NewTicket(TicketViewModel model, HttpContext httpContxt)
        {
            ApplicationUser user = await _userManager.GetUserAsync(httpContxt.User);
            Ticket ticket = _mapper.ViewModelToEntity(new Ticket(), model);
            ticket.Date = DateTime.Now;
            ticket.Active = true;
            ticket.UserId = user.Id;

            _ticketsRepository.Insert(ticket);
            await _ticketsRepository.Save();
        }

        public async Task ResponseToTicket(TicketResponseViewModel model, HttpContext httpContxt, int id)
        {
            TicketResponse ticketResponse = _mapperTicketsResponse.ViewModelToEntity(new TicketResponse(), model);
            ticketResponse.Date = DateTime.Now;
            ticketResponse.TicketId = id;

            _ticketsResponseRepository.Insert(ticketResponse);
            await _ticketsResponseRepository.Save();
        }

        public IEnumerable<TicketResponseViewModel> GetResponsesToTicket(int id)
        {
            List<TicketResponseViewModel> ticketResponseViewModels = new List<TicketResponseViewModel>();
            IEnumerable<TicketResponse> ticketResponses = _ticketsResponseRepository.GetAll().Where(a => a.TicketId == id);

            foreach (var item in ticketResponses)
            {
                ticketResponseViewModels.Add(_mapperTicketsResponse.EntityToViewModel(new TicketResponseViewModel(), item));
            }

            return ticketResponseViewModels.AsEnumerable();
        }
        
    }
}
