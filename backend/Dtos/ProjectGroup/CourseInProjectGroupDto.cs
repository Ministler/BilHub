using System;

namespace backend.Dtos.ProjectGroup
{
    public class CourseInProjectGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CourseInformation { get; set; }
        public bool IsActive { get; set; }
    }
}