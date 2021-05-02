using System;
using backend.Dtos.Comment;

namespace backend.Dtos.ProjectGroup
{
    public class GetGradeDto
    {
        public int GradingUserId { get; set; }
        public int Id { get; set; }
        public GetCommentorDto GradingUser { get; set; }
        public string Description { get; set; }
        public decimal MaxGrade { get; set; }
        public decimal Grade { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FileEndpoint { get; set; }
        public bool HasFile { get; set; }
    }
}