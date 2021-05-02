using backend.Models;

namespace backend.Dtos.Course
{
    public class InstructorInCourseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public UserTypeClass UserType { get; set; }
    }
}