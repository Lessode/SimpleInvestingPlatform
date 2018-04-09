using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Coins.Entities
{
    public class TicketResponse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TicketResponseId { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
