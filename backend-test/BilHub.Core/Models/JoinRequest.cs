using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Core.Models
{
    public class JoinRequest
    {
        public JoinRequest()
        {
            VotedStudents = new Collection<StudentJoinRequest>();
        }
        public int Id { get; set; }
        public Student RequestingStudent { get; set; }
        public ProjectGroup RequestedGroup { get; set; }
        public int AcceptedNumber { get; set; }
        public int RejectedNumber { get; set; }
        public ICollection<StudentJoinRequest> VotedStudents { get; set; }
    }
}