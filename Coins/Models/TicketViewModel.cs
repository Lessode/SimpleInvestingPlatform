using Coins.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coins.Models
{
    public class TicketViewModel
    {
        public int TicketId { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool Active { get; set; }
        public DateTime Date { get; set; }
    }
}
