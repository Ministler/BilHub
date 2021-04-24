namespace backend.Models
{
    public class GroupSize
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public Course AffiliatedCourse { get; set; }
        public int CourseId { get; set; }
    }
}