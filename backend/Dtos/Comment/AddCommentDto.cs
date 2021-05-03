using System;

namespace backend.Dtos.Comment
{
    public class AddCommentDto
    {
        public int CommentedSubmissionId { get; set; }
        public string CommentText { get; set; }
        public decimal MaxGrade { get; set; }
        public decimal Grade { get; set; }
    }
}