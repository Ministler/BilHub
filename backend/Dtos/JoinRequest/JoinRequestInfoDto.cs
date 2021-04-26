using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
 

namespace backend.Dtos.JoinRequest
{
    public class JoinRequestInfoDto
    {
        public int Id { get; set; }
        public bool Accepted { get; set; }
        public bool Resolved { get; set; }
        public string VotedStudents { get; set; }
    }
}