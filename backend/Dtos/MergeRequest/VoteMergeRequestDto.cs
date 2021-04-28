using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
 

namespace backend.Dtos.MergeRequest
{
    public class VoteMergeRequestDto
    {
        public int Id { get; set; }
        public bool accept { get; set; }
    }
}