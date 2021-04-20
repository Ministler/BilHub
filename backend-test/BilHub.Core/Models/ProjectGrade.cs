namespace BilHub.Core.Models
{
    public class ProjectGrade : GradeBase
    {
        public Student GradingStudent { get; set; }
        public ProjectGroup GradedProjectGroup { get; set; }
    }
}