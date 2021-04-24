namespace backend.Models
{
    public class InstructorCourse
    {
        public int InstructorId { get; set; }
        public User Instructor { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}