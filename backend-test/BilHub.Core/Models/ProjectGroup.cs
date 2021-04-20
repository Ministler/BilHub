using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Core.Models
{
    public class ProjectGroup
    {
        public ProjectGroup()
        {
            GroupMembers = new Collection<StudentProjectGroup>();
            Submissions = new Collection<Submission>();
            ProjectGradings = new Collection<ProjectGrade>();
            OutgoingMergeRequest = new Collection<MergeRequest>();
        }
        public int Id { get; set; }
        public Course AffiliatedCourse { get; set; }
        public bool ConfirmationState { get; set; }
        public int ConfirmedUserNumber { get; set; }
        public int GroupSize { get; set; }
        public string ProjectInformation { get; set; }
        public ICollection<StudentProjectGroup> GroupMembers { get; set; }
        public ICollection<Submission> Submissions { get; set; }
        public ICollection<ProjectGrade> ProjectGradings { get; set; }
        public ICollection<MergeRequest> OutgoingMergeRequest { get; set; }

    }
}