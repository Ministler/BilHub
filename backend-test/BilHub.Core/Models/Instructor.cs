using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Core.Models
{
    public class Instructor : User
    {
        public Instructor()
        {
            InstructedCourses = new Collection<InstructorCourse>();
        }
        public ICollection<InstructorCourse> InstructedCourses { get; set; }


    }
}