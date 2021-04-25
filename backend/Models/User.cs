using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class User
    {
        public User()
        {
            InstructedCourses = new Collection<Course>();
            ProjectGroups = new Collection<ProjectGroup>();
            OutgoingJoinRequests = new Collection<JoinRequest>();
            OutgoingComments = new Collection<Comment>();
        }
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string VerificationCode { get; set; } = "";
        public byte[] PasswordHash { get; set; }
        public byte[] SecondPasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool VerifiedStatus { get; set; }
        public bool DarkModeStatus { get; set; }
        public ICollection<Course> InstructedCourses { get; set; }
        public ICollection<ProjectGroup> ProjectGroups { get; set; }
        public ICollection<JoinRequest> OutgoingJoinRequests { get; set; }
        public ICollection<Comment> OutgoingComments { get; set; }
        [Required]
        public UserTypeClass UserType { get; set; } = UserTypeClass.Student;
    }
}