using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Models
{
    public class PeerGradeAssignment
    {
        public PeerGradeAssignment()
        {
            
            PeerGrades = new Collection<PeerGrade>();
        }
        public int Id { get; set; }
        public Course AffiliatedCourse { get; set; }
        public int CourseId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int MaxGrade { get; set; }
        public ICollection<PeerGrade> PeerGrades { get; set; }
        //public string AssignmentDescription { get; set; }
        //public DateTime Date { get; set; }
        // public User PublishedInstructor { get; set; }
        // public int UserId { get; set; }
    }
}