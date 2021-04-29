namespace backend.Dtos.Comment
{
    public class GetCommentDto
    {
        public int SubmissionId { get; set; }
        public int CommentId { get; set; }
        public string FileEndpoint { get; set; }
    }
}