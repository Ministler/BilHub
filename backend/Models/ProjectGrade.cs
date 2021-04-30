namespace backend.Models
{
    public class ProjectGrade : GradeBase
    {
        public User GradingUser { get; set; }
        public int GradingUserId { get; set; }
        public string Description { get; set; }
        public ProjectGroup GradedProjectGroup { get; set; }
        public int GradedProjectGroupID { get; set; }
        //public string FilePath { get; set; }
    }
}