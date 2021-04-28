using Microsoft.AspNetCore.Http;

namespace backend.Dtos.Assignment
{
    public class AddAssignmentFileDto
    {
        public IFormFile File { get; set; }
        public int AssignmentId { get; set; }
    }
}