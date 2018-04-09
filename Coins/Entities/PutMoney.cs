using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Coins.Entities
{
    public class PutMoney
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PutMoneyId { get; set; }
        public DateTime Date { get; set; }
        public bool Used { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Email { get; set; }
        public decimal Money { get; set; }
        public bool Active { get; set; }
    }
}
