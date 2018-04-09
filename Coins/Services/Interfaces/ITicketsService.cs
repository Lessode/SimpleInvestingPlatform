using Coins.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coins.Services.Interfaces
{
    public interface ITicketsService
    {
        Task NewTicket(TicketViewModel model, HttpContext httpContxt);
        Task Deactivate(int ticketId);
        Task<IEnumerable<TicketViewModel>> GetUserTickets(HttpContext httpContext);
        IEnumerable<TicketViewModel> GetAllTickets();
        Task<TicketViewModel> GetUserTicket(HttpContext httpContext, int id);
        Task ResponseToTicket(TicketResponseViewModel model, HttpContext httpContxt, int id);
        IEnumerable<TicketResponseViewModel> GetResponsesToTicket(int id);

    }
}
