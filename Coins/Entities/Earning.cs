using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Coins.Entities
{
    public class Earning
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EarningId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime Date { get; set; }
        public decimal Money { get; set; }
    }
}
