using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Core.Models
{
    public class Instructor
    {
        public Instructor()
        {
            InstructedCourses = new Collection<InstructorCourse>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Information { get; set; }
        public string VerifiedStatus { get; set; }
        public string LoginStatus { get; set; }
        public bool DarkModeStatus { get; set; }
        public ICollection<InstructorCourse> InstructedCourses { get; set; }


    }
}