using System;

namespace backend.Dtos.Assignment
{
    public class NotGradedAsssignmentsDto
    {
        public string courseCode { get; set; }
        public string assignmentName { get; set; }
        public DateTime dueDate { get; set; }
        public int courseId { get; set; }
        public int assignmentId { get; set; }
    }
}