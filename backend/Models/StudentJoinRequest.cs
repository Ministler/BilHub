namespace BilHub.Models
{
    public class StudentJoinRequest
    {
        public int StudentId { get; set; }
        public User Student { get; set; }
        public int JoinRequestId { get; set; }
        public JoinRequest JoinRequest { get; set; }
    }
}