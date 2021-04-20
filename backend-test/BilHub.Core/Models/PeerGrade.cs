namespace BilHub.Core.Models
{
    public class PeerGrade : GradeBase
    {
        public Student Reviewer { get; set; }
        public Student Reviewee { get; set; }
    }
}