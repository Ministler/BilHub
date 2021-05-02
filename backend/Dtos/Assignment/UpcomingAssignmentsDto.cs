using System;

namespace backend.Dtos.Assignment
{
    public class UpcomingAssignmentsDto
    {
        public string courseCode { get; set; }
        public string assignmentName { get; set; }
        public DateTime dueDate { get; set; }
        public int projectId { get; set; }
        public int submissionId { get; set; }

    }
}