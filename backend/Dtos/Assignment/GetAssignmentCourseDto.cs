using backend.Models;

namespace backend.Dtos.Assignment
{
    public class GetAssignmentCourseDto
    {
        public string name { get; set; }
        public SemesterType semester { get; set; }
        public int year { get; set; }
    }
}