namespace BilHub.Models
{
    public class PeerGrade : GradeBase
    {
        public User Reviewer { get; set; }
        public int ReviewerId { get; set; }
        public User Reviewee { get; set; }
        public int RevieweeId { get; set; }
    }
}