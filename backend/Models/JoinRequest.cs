using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Models
{
    public class JoinRequest
    {
        public JoinRequest()
        {
            VotedStudents = "";
        }
        public int Id { get; set; }
        public User RequestingStudent { get; set; }
        public int UserId { get; set; }
        public ProjectGroup RequestedGroup { get; set; }
        public int ProjectGroupId { get; set; }
        public int AcceptedNumber { get; set; }
        public int RejectedNumber { get; set; }
        public string VotedStudents { get; set; }
        // public Course AffiliatedCourse { get; set; }
        // public int CourseId { get; set; }
        // public Section AffiliatedSection { get; set; }
        // public int Section { get; set; }
    }
}