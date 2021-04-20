namespace BilHub.Core.Models
{
    public class StudentMergeRequest
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int MergeRequestId { get; set; }
        public MergeRequest MergeRequest { get; set; }
    }
}