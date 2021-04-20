namespace BilHub.Core.Models
{
    public class StudentProjectGroup
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int ProjectGroupId { get; set; }
        public ProjectGroup ProjectGroup { get; set; }
    }
}