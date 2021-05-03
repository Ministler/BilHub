
namespace backend.Models
{
    public class CourseUser
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public Course Course { get; set; }
        public int CourseId { get; set; }
    }
}