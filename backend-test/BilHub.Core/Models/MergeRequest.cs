namespace BilHub.Core.Models
{
    public class MergeRequest
    {
        public ProjectGroup SenderGroup { get; set; }
        public ProjectGroup ReceiverGroup { get; set; }
        public int AcceptedNumberInSender { get; set; }
        public int RejectedNumberInSender { get; set; }
        public int AcceptedNumberInReceiver { get; set; }
        public int RejectedNumberInReceiver { get; set; }
    }
}