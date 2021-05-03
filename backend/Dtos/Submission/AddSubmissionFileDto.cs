using Microsoft.AspNetCore.Http;

namespace backend.Dtos.Submission
{
    public class AddSubmissionFileDto
    {
        public IFormFile File { get; set; }
        public int SubmissionId { get; set; }
    }
}