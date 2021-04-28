using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Models
{
    public class JoinRequest
    {
        public JoinRequest()
        {
            VotedStudents = "";
        }
        public int Id { get; set; }
        public User RequestingStudent { get; set; }
        public int RequestingStudentId { get; set; }
        public string Description { get; set; }
        public ProjectGroup RequestedGroup { get; set; }
        public int RequestedGroupId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AcceptedNumber { get; set; }
        public bool Accepted { get; set; }
        public bool Resolved { get; set; }
        public string VotedStudents { get; set; }
        public string Description { get; set; }
        // public Course AffiliatedCourse { get; set; }
        // public int CourseId { get; set; }
        // public Section AffiliatedSection { get; set; }
        // public int Section { get; set; }
    }
}