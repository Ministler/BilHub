using System;

namespace BilHub.Core.Models
{
    public class PeerGradeAssignment
    {
        public User PublishedUser { get; set; }
        public Byte[] AssignmentDescription { get; set; }
        public DateTime Date { get; set; }
    }
}