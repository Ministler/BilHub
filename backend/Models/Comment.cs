using System;

namespace BilHub.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public User CommentedUser { get; set; }
        public Submission CommentedSubmission { get; set; }
        public int SubmissionId { get; set; }
        public string CommentText { get; set; }
        public bool GradeStatus { get; set; }
        public double MaxGrade { get; set; }
        public double Grade { get; set; }
        public bool FileAttachmentAvailability { get; set; }
        public string FilePath { get; set; }
    }
}