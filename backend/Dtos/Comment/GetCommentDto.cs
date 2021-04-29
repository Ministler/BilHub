using System;

namespace backend.Dtos.Comment
{
    public class GetCommentDto
    {
        public int CommentedSubmissionId { get; set; }
        public int CommentId { get; set; }
        public GetCommentorDto CommentedUser { get; set; }
        public string CommentText { get; set; }
        public decimal MaxGrade { get; set; }
        public decimal Grade { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FileEndpoint { get; set; }
        public bool HasFile { get; set; }
    }
}