using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Models
{
    public class Section
    {
        public Section()
        {
            ProjectGroups = new Collection<ProjectGroup>();
            // JoinRequests = new Collection<JoinRequest>();
            // MergeRequests = new Collection<MergeRequest>();
        }
        public int Id { get; set; }
        public bool SectionlessState { get; set; }
        public int SectionNo { get; set; }
        public ICollection<ProjectGroup> ProjectGroups { get; set; }
        public Course AffiliatedCourse { get; set; }
        public int AffiliatedCourseId { get; set; }
        // public ICollection<JoinRequest> JoinRequests { get; set; }
        // public ICollection<MergeRequest> MergeRequests { get; set; }

    }
}