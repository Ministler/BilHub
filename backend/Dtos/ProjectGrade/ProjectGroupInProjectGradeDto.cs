using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Dtos.ProjectGrade
{
    public class ProjectGroupInProjectGradeDto
    {
        public int Id { get; set; }
        public int AffiliatedSectionId { get; set; }
        public int AffiliatedCourseId { get; set; }
        public bool ConfirmationState { get; set; }
        public int ConfirmedUserNumber { get; set; }
        public string ProjectInformation { get; set; }
        public string ConfirmedGroupMembers { get; set; }
    }
}