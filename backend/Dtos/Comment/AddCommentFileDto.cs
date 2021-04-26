using Microsoft.AspNetCore.Http;

namespace backend.Dtos.Comment
{
    public class AddCommentFileDto
    {
        public int SubmissionId { get; set; }
        public int CommentId { get; set; }
        public IFormFile CommentFile { get; set; }
    }
}