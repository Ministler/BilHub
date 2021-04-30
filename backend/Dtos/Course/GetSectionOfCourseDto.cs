namespace backend.Dtos.Section
{
    public class GetSectionOfCourseDto
    {
        public int Id { get; set; }
        public int SectionNo { get; set; }
        public int AffiliatedCourseId { get; set; }
    }
}