using System;

namespace BilHub.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public User CommentedUser { get; set; }
        public Submission CommentedSubmission { get; set; }
        public string CommentText { get; set; }
        public bool GradeStatus { get; set; }
        public double MaxGrade { get; set; }
        public double Grade { get; set; }
        public bool FileAttachmentAvailability { get; set; }
        public Byte[] File { get; set; }
    }
}