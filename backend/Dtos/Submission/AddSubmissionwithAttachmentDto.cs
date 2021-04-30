using Microsoft.AspNetCore.Http;

namespace backend.Dtos.Submission
{
    public class AddSubmissionwithAttachmentDto
    {
        public IFormFile file { get; set; }
        public AddSubmissionDto addSubmissionDto { get; set; }
    }
}