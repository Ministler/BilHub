using System;

namespace backend.Dtos.Course
{
    public class EditCourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CourseSemester { get; set; }
        public int Year { get; set; }
        public string CourseInformation { get; set; }
        public DateTime LockDate { get; set; }
        public int MinGroupSize { get; set; }
        public int MaxGroupSize { get; set; }
    }
}