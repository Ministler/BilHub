namespace backend.Dtos.JoinRequest
{
    public class JoinRequestResultDto
    {
        public int UserId { get; set; }
        public int ProjectGroupId { get; set; }
        public int AcceptedNumber { get; set; }
        public int RejectedNumber { get; set; }
        public string VotedStudents { get; set; }
    }
}