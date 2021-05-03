using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Dtos.MergeRequest
{
    public class ProjectGroupInMergeRequestDto
    {
        public int Id { get; set; }
        public int AffiliatedSectionId { get; set; }
        public int AffiliatedCourseId { get; set; }
        public bool ConfirmationState { get; set; }
        public int ConfirmedUserNumber { get; set; }
        public string ProjectInformation { get; set; }
        public string ConfirmedGroupMembers { get; set; }
        public ICollection<GroupMemberInMergeRequestDto> GroupMembers {get; set;}
    }
}