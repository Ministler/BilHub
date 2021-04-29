using Microsoft.AspNetCore.Http;

namespace backend.Dtos.Assignment
{
    public class UpdateAssignmentWithAttachment
    {

        public IFormFile file { get; set; }
        public UpdateAssignmentDto updateAssignmentDto { get; set; }
    }
}