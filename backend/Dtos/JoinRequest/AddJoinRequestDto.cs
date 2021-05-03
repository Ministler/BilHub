using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
 
// why do we use time??

namespace backend.Dtos.JoinRequest
{
    public class AddJoinRequestDto
    {
        public int RequestedGroupId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
    }
}