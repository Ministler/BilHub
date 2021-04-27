using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Dtos.JoinRequest
{
    public class GetJoinRequestDto
    {
        public int Id { get; set; }
        public UserInJoinRequestDto RequestingStudent { get; set; }
        public int RequestingStudentId { get; set; }
        public ProjectGroupInJoinRequestDto RequestedGroup { get; set; }
        public int RequestedGroupId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AcceptedNumber { get; set; }
        public bool Accepted { get; set; }
        public bool Resolved { get; set; }
        public string VotedStudents { get; set; }
    }
}