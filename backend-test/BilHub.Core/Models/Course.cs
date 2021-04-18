using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Core.Models
{
    public class Course
    {
        public Course()
        {
            Students = new Collection<Student>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int InstructorId { get; set; }
        public string description { get; set; }
        public Instructor Instructor { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}