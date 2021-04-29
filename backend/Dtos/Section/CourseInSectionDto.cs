using backend.Models;

namespace backend.Dtos.Section
{
    public class CourseInSectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SemesterType CourseSemester { get; set; }
        public int Year { get; set; }
    }
}