using Coins.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Coins.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Earning> Earnings { get; set; }
        public DbSet<PutMoney> PutMoneys { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketResponse> TicketResponses { get; set; }
        public DbSet<Setting> Settings { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
