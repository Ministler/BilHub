using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Core.Models
{
    public class Instructor
    {
        public Instructor()
        {
            Courses = new Collection<Course>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; }


    }
}