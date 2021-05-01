using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using backend.Dtos.GradeBase;
 
namespace backend.Dtos.PeerGrade
{
    public class GetPeerGradeDto : GetGradeBaseDto
    {
        public int ProjectGroupId { get; set; }
        public int ReviewerId { get; set; }
        public int RevieweeId { get; set; }
    }
}