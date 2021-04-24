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
        // public User PublishedUser { get; set; }
        // public int UserId { get; set; }
        public Section AffiliatedSection { get; set; }
        public int SectionId { get; set; }
        public string AssignmentDescriptionFile { get; set; }
        public DateTime Date { get; set; }
        public string AcceptedTypes { get; set; }
        public int MaxFileSizeInBytes { get; set; }
        public bool VisibilityOfSubmission { get; set; }
        public bool CanBeGradedByStudents { get; set; }
        public ICollection<Submission> Submissions { get; set; }

    }
}