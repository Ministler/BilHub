using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Models
{
    public class Assignment
    {
        public Assignment()
        {
            Submissions = new Collection<Submission>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public Course AfilliatedCourse { get; set; }
        public int AfilliatedCourseId { get; set; }
        public string AssignmentDescription { get; set; }
        public string FilePath { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AcceptedTypes { get; set; }
        public int MaxFileSizeInBytes { get; set; }
        public bool VisibilityOfSubmission { get; set; }
        public bool CanBeGradedByStudents { get; set; }
        public bool IsItGraded { get; set; }
        public decimal MaxGrade { get; set; }
        public bool HasFile { get; set; }
        public ICollection<Submission> Submissions { get; set; }
        // public User PublishedUser { get; set; }
        // public int UserId { get; set; }

    }
}