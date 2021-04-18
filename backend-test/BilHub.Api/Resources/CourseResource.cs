namespace BilHub.Api.Resources
{
    public class CourseResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public InstructorResource Instructor { get; set; }
        public string description { get; set; }
    }
}