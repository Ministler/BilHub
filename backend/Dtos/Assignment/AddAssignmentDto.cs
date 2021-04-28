using System;

namespace backend.Dtos.Assignment
{
    public class AddAssignmentDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string AssignmenDescription { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreateTime { get; set; }
        public string AcceptedTypes { get; set; }
        public int MaxFileSizeInBytes { get; set; }
        public bool VisibiltyOfSubmission { get; set; }
        public bool CanBeGradedByStudents { get; set; }
        public bool IsItGraded { get; set; }
    }
}