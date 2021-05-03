using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Dtos.MergeRequest
{
    public class GetMergeRequestDto
    {
        public int Id { get; set; }
        public ProjectGroupInMergeRequestDto SenderGroup { get; set; }
        public int SenderGroupId { get; set; }
        public ProjectGroupInMergeRequestDto ReceiverGroup { get; set; }
        public int ReceiverGroupId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Accepted { get; set; }
        public bool Resolved { get; set; }
        public string VotedStudents { get; set; }
        public string Description { get; set; }
        public string CourseName {get;set;}
        public DateTime LockDate {get; set;}
        public bool CurrentUserVote {get; set;}
    }
}