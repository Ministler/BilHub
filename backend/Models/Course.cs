using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Models
{
    public class Course
    {
        public Course()
        {
            Sections = new Collection<Section>();
            Instructors = new Collection<User>();
            GroupSizes = new Collection<GroupSize>();
            // ProjectGrades = new Collection<ProjectGrade>();
            // ProjectGroups = new Collection<ProjectGroup>();
            // Assignments = new Collection<Assignment>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string CourseInformation { get; set; }
        public DateTime LockDate { get; set; }
        public string CourseSemester { get; set; }
        public PeerGradeAssignment PeerGradeAssignment { get; set; }
        public ICollection<GroupSize> GroupSizes { get; set; }
        public ICollection<Section> Sections { get; set; }
        public ICollection<User> Instructors { get; set; }
        // public ICollection<Assignment> Assignments { get; set; }
        // public ICollection<ProjectGroup> ProjectGroups { get; set; }

    }
}