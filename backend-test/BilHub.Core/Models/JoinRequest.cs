namespace BilHub.Core.Models
{
    public class JoinRequest
    {
        public Student RequestingStudent { get; set; }
        public ProjectGroup RequestedGroup { get; set; }
        public int AcceptedNumber { get; set; }
        public int RejectedNumber { get; set; }
    }
}