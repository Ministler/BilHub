using Microsoft.AspNetCore.Http;

namespace BilHub.Dtos.Submission
{
    public class AddSubmissionDto
    {
        public IFormFile file { get; set; }
        public string Status { get; set; }
    }
}