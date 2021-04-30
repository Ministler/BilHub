using System;

namespace backend.Dtos.Assignment
{
    public class GetAssignmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AfilliatedCourseId { get; set; }
        public string AssignmentDescription { get; set; }
        public string FileEndpoint { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AcceptedTypes { get; set; }
        public int MaxFileSizeInBytes { get; set; }
        public bool VisibilityOfSubmission { get; set; }
        public bool CanBeGradedByStudents { get; set; }
        public bool IsItGraded { get; set; }
        public bool HasFile { get; set; }
    }
}