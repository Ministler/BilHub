using System;

namespace backend.Dtos.Assignment
{
    public class GetFeedItemDto
    {
        public string title { get; set; }
        public int status { get; set; }
        public string caption { get; set; }
        public string publisher { get; set; }
        public int publisherId { get; set; }
        public DateTime publishmentDate { get; set; }
        public DateTime dueDate { get; set; }
        public bool hasFile { get; set; }
        public string fileEndpoint { get; set; }
        public int? projectId { get; set; }
        public int? submissionId { get; set; }
        public int? courseId { get; set; }
        public int? assignmentId { get; set; }
    }
}