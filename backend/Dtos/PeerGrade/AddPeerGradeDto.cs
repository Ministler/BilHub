using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using backend.Dtos.GradeBase;
 
namespace backend.Dtos.PeerGrade
{
    public class AddPeerGradeDto : AddGradeBaseDto
    {
        public int ProjectGroupId { get; set; }
        public int RevieweeId { get; set; }

    }
}