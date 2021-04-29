using Microsoft.AspNetCore.Http;

namespace backend.Dtos.Submission
{
    public class UpdateSubmissionWithAttachment
    {

        public IFormFile file { get; set; }
        public UpdateSubmissionDto updateSubmissionDto { get; set; }
    }
}