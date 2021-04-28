using Microsoft.AspNetCore.Http;

namespace backend.Dtos.Comment
{
    public class AddCommentFileDto
    {
        public int CommentId { get; set; }
        public IFormFile CommentFile { get; set; }
    }
}