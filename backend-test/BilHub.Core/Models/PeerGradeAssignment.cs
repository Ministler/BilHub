using System;

namespace BilHub.Core.Models
{
    public class PeerGradeAssignment
    {
        public int Id { get; set; }
        public User PublishedUser { get; set; }
        public Byte[] AssignmentDescription { get; set; }
        public DateTime Date { get; set; }
    }
}