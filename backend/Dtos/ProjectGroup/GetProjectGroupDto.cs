using System.Collections.Generic;

namespace backend.Dtos.ProjectGroup
{
    public class GetProjectGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SectionInProjectGroupDto AffiliatedSection { get; set; }
        public int AffiliatedSectionId { get; set; }
        public CourseInProjectGroupDto AffiliatedCourse { get; set; }
        public int AffiliatedCourseId { get; set; }
        public bool ConfirmationState { get; set; }
        public int ConfirmedUserNumber { get; set; }
        public string ProjectInformation { get; set; }
        public ICollection<UserInProjectGroupDto> GroupMembers { get; set; }
        public string ConfirmedGroupMembers { get; set; }
        public bool IsActive { get; set; } = true;
        public string AffiliatedCourseName { get; set; }
        public bool ConfirmStateOfCurrentUser {get; set;}
    }
}