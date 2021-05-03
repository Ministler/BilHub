using System;

namespace backend.Dtos.Submission
{
    public class AddSubmissionDto
    {
        public int ProjectGroupId { get; set; }
        public int AssignmentId { get; set; }
        public string Description { get; set; }
    }
}