using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BilHub.Core.Models
{
    public class MergeRequest
    {
        public MergeRequest()
        {
            VotedStudents = new Collection<Student>();
        }
        public int Id { get; set; }
        public ProjectGroup SenderGroup { get; set; }
        public ProjectGroup ReceiverGroup { get; set; }
        public int AcceptedNumberInSender { get; set; }
        public int RejectedNumberInSender { get; set; }
        public int AcceptedNumberInReceiver { get; set; }
        public int RejectedNumberInReceiver { get; set; }
        public ICollection<Student> VotedStudents { get; set; }
    }
}