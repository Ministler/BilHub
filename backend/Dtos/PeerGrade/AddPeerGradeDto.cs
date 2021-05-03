using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using backend.Dtos.GradeBase;
 
namespace backend.Dtos.PeerGrade
{
    public class AddPeerGradeDto
    {
        public int ProjectGroupId { get; set; }
        public int RevieweeId { get; set; }

        public decimal Grade { get; set; }
        public string Comment { get; set; }
        public DateTime LastEdited { get; set; }

    }
}