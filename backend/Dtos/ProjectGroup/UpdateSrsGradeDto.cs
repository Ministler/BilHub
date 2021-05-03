namespace backend.Dtos.ProjectGroup
{
    public class UpdateSrsGradeDto
    {
        public decimal SrsGrade { get; set; }
        public decimal MaxSrsGrade { get; set; }
        public int ProjectGroupId { get; set; }
    }
}