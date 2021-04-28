using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
 

namespace backend.Dtos.MergeRequest
{
    public class MergeRequestInfoDto
    {
        public int Id { get; set; }
        public bool Accepted { get; set; }
        public bool Resolved { get; set; }
        public string VotedStudents { get; set; }
    }
}