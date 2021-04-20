using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static System.Collections.Specialized.BitVector32;

namespace BilHub.Core.Models
{
    public class Course
    {
        public Course()
        {
            Sections = new Collection<Section>();
            Instructors = new Collection<InstructorCourse>();
            Assistants = new Collection<StudentCourse>();
            Students = new Collection<StudentCourse>();
            Assignments = new Collection<Assignment>();
            Submissions = new Collection<Submission>();
            GroupSizes = new Collection<int>();
            ProjectGrades = new Collection<ProjectGrade>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string CourseInformation { get; set; }
        public DateTime LockDate { get; set; }
        public string CourseSemester { get; set; }
        public PeerGradeAssignment PeerGradeAssignment { get; set; }
        public ICollection<int> GroupSizes { get; set; }
        public ICollection<ProjectGrade> ProjectGrades { get; set; }
        public ICollection<Section> Sections { get; set; }
        public ICollection<InstructorCourse> Instructors { get; set; }
        public ICollection<StudentCourse> Assistants { get; set; }
        public ICollection<StudentCourse> Students { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Submission> Submissions { get; set; }

    }
}