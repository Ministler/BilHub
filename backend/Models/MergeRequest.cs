using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Models
{
    public class MergeRequest
    {
        public MergeRequest()
        {
            VotedStudents = "";
        }
        public int Id { get; set; }
        public ProjectGroup SenderGroup { get; set; }
        public int SenderGroupId { get; set; }
        public string Description { get; set; }
        public ProjectGroup ReceiverGroup { get; set; }
        public int ReceiverGroupId { get; set; }
        public string VotedStudents { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Accepted { get; set; }
        public bool Resolved { get; set; }
        public enum MergeRequestType
        {
            Pending,
            Unresolved,
            Resolved
        }

        // public Course AffiliatedCourse { get; set; }
        // public int CourseId { get; set; }
        // public Section AffiliatedSection { get; set; }
        // public int SectionId { get; set; }
        // public int AcceptedNumberInSender { get; set; }
        // public int RejectedNumberInSender { get; set; }
        // public int AcceptedNumberInReceiver { get; set; }
        // public int RejectedNumberInReceiver { get; set; }

    }
}