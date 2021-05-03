using System;

namespace backend.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public User CommentedUser { get; set; }
        public int CommentedUserId { get; set; }
        public Submission CommentedSubmission { get; set; }
        public int CommentedSubmissionId { get; set; }
        public string CommentText { get; set; }
        public decimal MaxGrade { get; set; }
        public decimal Grade { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool FileAttachmentAvailability { get; set; }
        public string FilePath { get; set; }
    }
}