namespace BilHub.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Information { get; set; }
        public string VerifiedStatus { get; set; }
        public string LoginStatus { get; set; }
        public bool DarkModeStatus { get; set; }
    }
}