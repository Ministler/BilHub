using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class User
    {
        public User()
        {
            InstructedCourses = new Collection<CourseUser>();
            ProjectGroups = new Collection<ProjectGroupUser>();
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
        public string UserInformation { get; set; }
        public ICollection<CourseUser> InstructedCourses { get; set; }
        public ICollection<ProjectGroupUser> ProjectGroups { get; set; }
        public ICollection<JoinRequest> OutgoingJoinRequests { get; set; }
        public ICollection<Comment> OutgoingComments { get; set; }
        [Required]
        public UserTypeClass UserType { get; set; } = UserTypeClass.Student;
        public string ProfileInfo {get; set;}
    }
}