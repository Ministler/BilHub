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
            OutgoingJoinRequests = new Collection<StudentJoinRequest>();
            UnvotedJoinRequests = new Collection<StudentJoinRequest>();
            UnvotedMergeRequests = new Collection<StudentMergeRequest>();
        }
        public ICollection<StudentCourse> AssistedCourses { get; set; }
        public ICollection<StudentCourse> TakenCourses { get; set; }
        public ICollection<StudentProjectGroup> ProjectGroups { get; set; }
        public ICollection<StudentJoinRequest> OutgoingJoinRequests { get; set; }
        public ICollection<StudentJoinRequest> UnvotedJoinRequests { get; set; }
        public ICollection<StudentMergeRequest> UnvotedMergeRequests { get; set; }
    }
}