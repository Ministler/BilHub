using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Core.Models
{
    public class Student
    {
        public Student()
        {
            AssistedCourses = new Collection<AssistantCourse>();
            TakenCourses = new Collection<StudentCourse>();
            ProjectGroups = new Collection<StudentProjectGroup>();
            OutgoingJoinRequests = new Collection<StudentJoinRequest>();
            UnvotedJoinRequests = new Collection<UnvotedJoinRequest>();
            UnvotedMergeRequests = new Collection<StudentMergeRequest>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Information { get; set; }
        public string VerifiedStatus { get; set; }
        public string LoginStatus { get; set; }
        public bool DarkModeStatus { get; set; }
        public ICollection<AssistantCourse> AssistedCourses { get; set; }
        public ICollection<StudentCourse> TakenCourses { get; set; }
        public ICollection<StudentProjectGroup> ProjectGroups { get; set; }
        public ICollection<StudentJoinRequest> OutgoingJoinRequests { get; set; }
        public ICollection<UnvotedJoinRequest> UnvotedJoinRequests { get; set; }
        public ICollection<StudentMergeRequest> UnvotedMergeRequests { get; set; }
    }
}