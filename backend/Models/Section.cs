using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Models
{
    public class Section
    {
        public Section()
        {
            ProjectGroups = new Collection<ProjectGroup>();
            Students = new Collection<User>();
            // JoinRequests = new Collection<JoinRequest>();
            // MergeRequests = new Collection<MergeRequest>();
        }
        public int Id { get; set; }
        public bool SectionlessState { get; set; }
        public int SectionNo { get; set; }
        public ICollection<ProjectGroup> ProjectGroups { get; set; }
        public ICollection<User> Students { get; set; }
        // public ICollection<JoinRequest> JoinRequests { get; set; }
        // public ICollection<MergeRequest> MergeRequests { get; set; }

    }
}