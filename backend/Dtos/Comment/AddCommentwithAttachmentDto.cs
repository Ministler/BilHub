using Microsoft.AspNetCore.Http;

namespace backend.Dtos.Comment
{
    public class AddCommentwithAttachmentDto
    {
        public IFormFile File { get; set; }
        public AddCommentDto AddCommentDto { get; set; }
    }
}