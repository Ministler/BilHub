using System;

namespace backend.Dtos.Assignment
{
    public class UpdateAssignmentDto
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string AssignmenDescription { get; set; }
        public DateTime DueDate { get; set; }
        public string AcceptedTypes { get; set; }
        public int MaxFileSizeInBytes { get; set; }
        public bool VisibiltyOfSubmission { get; set; }
        public bool CanBeGradedByStudents { get; set; }
        public bool IsItGraded { get; set; }
    }
}