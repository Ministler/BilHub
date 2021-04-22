namespace BilHub.Core.Models
{
    public class ProjectGrade : GradeBase
    {
        public int MyProperty { get; set; }
        public Student GradingStudent { get; set; }
        public ProjectGroup GradedProjectGroup { get; set; }
    }
}