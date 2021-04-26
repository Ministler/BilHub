using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Models
{
    public class ProjectGroup
    {
        public ProjectGroup()
        {
            GroupMembers = new Collection<User>();
            Submissions = new Collection<Submission>();
            ProjectGrades = new Collection<ProjectGrade>();
            PeerGrades = new Collection<PeerGrade>();
            IncomingJoinRequests = new Collection<JoinRequest>();
            OutgoingMergeRequest = new Collection<MergeRequest>();
            IncomingMergeRequest = new Collection<MergeRequest>();
        }
        public int Id { get; set; }
        public Section AffiliatedSection { get; set; }
        public int AffiliatedSectionId { get; set; }
        public Course AffiliatedCourse { get; set; }
        public int AffiliatedCourseId { get; set; }
        public bool ConfirmationState { get; set; }
        public int ConfirmedUserNumber { get; set; }
        public string ProjectInformation { get; set; }
        public ICollection<User> GroupMembers { get; set; }
        public ICollection<ProjectGrade> ProjectGrades { get; set; }
        public ICollection<PeerGrade> PeerGrades { get; set; }
        public ICollection<Submission> Submissions { get; set; }
        public ICollection<JoinRequest> IncomingJoinRequests { get; set; }
        public ICollection<MergeRequest> OutgoingMergeRequest { get; set; }
        public ICollection<MergeRequest> IncomingMergeRequest { get; set; }
        // public Section AffiliatedCourse { get; set; }
        // public int CourseId { get; set; }       


    }
}