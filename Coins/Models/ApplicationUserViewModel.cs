namespace Coins.Models
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public decimal Balance { get; set; }
        public decimal Deposit { get; set; }
        public string Refferal { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
