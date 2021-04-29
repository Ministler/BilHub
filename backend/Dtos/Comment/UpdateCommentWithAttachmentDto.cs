using Microsoft.AspNetCore.Http;

namespace backend.Dtos.Comment
{
    public class UpdateCommentWithAttachmentDto
    {
        public IFormFile File { get; set; }
        public UpdateCommentDto updateCommentDto { get; set; }
    }
}