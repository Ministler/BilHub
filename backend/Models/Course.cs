using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Models
{
    public class Course
    {
        public Course()
        {
            Sections = new Collection<Section>();
            Instructors = new Collection<CourseUser>();
            Assignments = new Collection<Assignment>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public SemesterType CourseSemester { get; set; }
        public int Year { get; set; }
        public string CourseInformation { get; set; }
        public string CourseDescription { get; set; }
        public int NumberOfSections { get; set; }
        public bool IsSectionless { get; set; } = false;
        public DateTime LockDate { get; set; }
        public DateTime StartDate { get; set; }
        public int MinGroupSize { get; set; }
        public int MaxGroupSize { get; set; }
        public bool IsActive { get; set; } = false;
        public PeerGradeAssignment PeerGradeAssignment { get; set; }
        public ICollection<Section> Sections { get; set; }
        public ICollection<CourseUser> Instructors { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public bool IsLocked { get; set; }
    }
}