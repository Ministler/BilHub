namespace backend.Models
{
    public class ProjectGrade : GradeBase
    {
        public User GradingUser { get; set; }
        public int UserId { get; set; }
        public ProjectGroup GradedProjectGroup { get; set; }
        public int ProjectGroupID { get; set; }
    }
}