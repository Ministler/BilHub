using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
 

namespace backend.Dtos.JoinRequest
{
    public class VoteJoinRequestDto
    {
        public int Id { get; set; }
        //public int voterId { get; set; }
        public bool accept { get; set; }
    }
}