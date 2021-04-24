using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BilHub.Models
{
    public class User
    {
        public User()
        {
            InstructedCourses = new Collection<Course>();
            ProjectGroups = new Collection<ProjectGroup>();
            OutgoingPeerGrades = new Collection<PeerGrade>();
            IncomingPeerGrades = new Collection<PeerGrade>();
            OutgoingJoinRequests = new Collection<JoinRequest>();
            OutgoingComments = new Collection<Comment>();
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] SecondPasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool VerifiedStatus { get; set; }
        public bool LoginStatus { get; set; }
        public bool DarkModeStatus { get; set; }
        public ICollection<Course> InstructedCourses { get; set; }
        public ICollection<ProjectGroup> ProjectGroups { get; set; }
        public ICollection<PeerGrade> OutgoingPeerGrades { get; set; }
        public ICollection<PeerGrade> IncomingPeerGrades { get; set; }
        public ICollection<JoinRequest> OutgoingJoinRequests { get; set; }
        public ICollection<Comment> OutgoingComments { get; set; }
        [Required]
        public int UserType { get; set; }
    }
}