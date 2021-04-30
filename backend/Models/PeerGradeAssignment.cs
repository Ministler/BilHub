using System;

namespace backend.Models
{
    public class PeerGradeAssignment
    {
        public int Id { get; set; }
        public Course AffiliatedCourse { get; set; }
        public int CourseId { get; set; }
        public string AssignmentDescription { get; set; }
        public DateTime Date { get; set; }
        // public User PublishedInstructor { get; set; }
        // public int UserId { get; set; }
    }
}