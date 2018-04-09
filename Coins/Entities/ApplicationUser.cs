using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel;

namespace Coins.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public decimal Balance { get; set; }
        public decimal Deposit { get; set; }
        public string Refferal { get; set; }
    }
}
