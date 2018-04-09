using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Coins.Entities
{
    public class Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public decimal Money { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
    }
}
