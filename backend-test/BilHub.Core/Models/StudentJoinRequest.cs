namespace BilHub.Core.Models
{
    public class StudentJoinRequest
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int JoinRequestId { get; set; }
        public JoinRequest JoinRequest { get; set; }
    }
}