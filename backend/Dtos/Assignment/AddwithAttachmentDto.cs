using Microsoft.AspNetCore.Http;

namespace backend.Dtos.Assignment
{
    public class AddwithAttachmentDto
    {
        public IFormFile file { get; set; }
        public AddAssignmentDto addAssignmentDto { get; set; }
    }
}