namespace BilHub.Api.Resources
{
    public class StudentResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseResource Course { get; set; }
    }
}