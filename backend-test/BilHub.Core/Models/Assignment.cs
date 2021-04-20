using System;

namespace BilHub.Core.Models
{
    public class Assignment
    {
        public User PublishedUsed { get; set; }
        public Byte[] AssignmentDescription { get; set; }
        public DateTime Date { get; set; }
        public string AcceptedTypes { get; set; }
        public int MaxFileSizeInBytes { get; set; }
        public bool VisibilityOfSubmission { get; set; }
        public bool CanBeGradedByStudents { get; set; }

    }
}