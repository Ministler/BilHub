namespace BilHub.Core.Models
{
    public class UnvotedJoinRequest
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int JoinRequestId { get; set; }
        public JoinRequest JoinRequest { get; set; }
    }
}