namespace backend.Models
{
    public class ProjectGroupUser
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public ProjectGroup ProjectGroup { get; set; }
        public int ProjectGroupId { get; set; }
    }
}