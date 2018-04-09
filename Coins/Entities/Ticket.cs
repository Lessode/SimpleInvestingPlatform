using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Coins.Entities
{
    public class Ticket
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool Active { get; set; }
        public DateTime Date { get; set; }
    }
}
