namespace backend.Models
{
    public class PeerGrade : GradeBase
    {
        public int CourseId { get; set; }
        public int ReviewerId { get; set; }
        public int RevieweeId { get; set; }
    }
}