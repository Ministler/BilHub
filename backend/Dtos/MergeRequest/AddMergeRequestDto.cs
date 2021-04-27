using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
 
namespace backend.Dtos.MergeRequest
{
    public class AddMergeRequestDto
    {
        public int ReceiverGroupId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
    }
}