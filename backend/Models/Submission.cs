using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Models
{
    public class Submission
    {
        public Submission()
        {
            Comments = new Collection<Comment>();
        }
        public int Id { get; set; }
        public Assignment AffiliatedAssignment { get; set; }
        public ProjectGroup AffiliatedGroup { get; set; }
        public int ProjectGroupId { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public ICollection<Comment> Comments { get; set; }
        // public Course AffiliatedCourse { get; set; }
        // public int CourseId { get; set; }
    }
}