using System;

namespace backend.Dtos.Submission
{
    public class AddSubmissionDto
    {
        public string Description { get; set; }
        public int AffiliatedAssignmentId { get; set; }
    }
}