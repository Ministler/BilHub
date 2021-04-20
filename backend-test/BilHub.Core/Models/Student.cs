using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Core.Models
{
    public class Student : User
    {
        public Student()
        {
            AssistedCourses = new Collection<StudentCourse>();
            TakenCourses = new Collection<StudentCourse>();
            ProjectGroups = new Collection<StudentProjectGroup>();
            OutgoingJoinRequests = new Collection<JoinRequest>();
        }
        public ICollection<StudentCourse> AssistedCourses { get; set; }
        public ICollection<StudentCourse> TakenCourses { get; set; }
        public ICollection<StudentProjectGroup> ProjectGroups { get; set; }
        public ICollection<JoinRequest> OutgoingJoinRequests { get; }
    }
}