namespace backend.Dtos.Submission
{
    public class GetSubmissionsFileDto
    {
        public int CourseId { get; set; }
        public int SectionId { get; set; }
        public int AssignmentId { get; set; }
    }
}