namespace backend.Dtos.Comment
{
    public class UpdateCommentDto
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public decimal MaxGrade { get; set; }
        public decimal Grade { get; set; }
    }
}