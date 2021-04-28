using Microsoft.AspNetCore.Http;

namespace backend.Dtos.Comment
{
    public class UpdateCommentWithAttachmentDto
    {
        public int CommentId { get; set; }
        public IFormFile File { get; set; }
        public AddCommentDto AddCommentDto { get; set; }
    }
}