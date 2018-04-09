using Coins.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coins.Models
{
    public class TicketResponseViewModel
    {
        public string TicketResponseId { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
