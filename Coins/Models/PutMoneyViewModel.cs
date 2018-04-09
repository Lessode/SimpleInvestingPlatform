using Coins.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coins.Models
{
    public class PutMoneyViewModel
    {
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
