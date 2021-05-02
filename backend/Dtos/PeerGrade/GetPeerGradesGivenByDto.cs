using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using backend.Dtos.GradeBase;
 
namespace backend.Dtos.PeerGrade
{
    public class GetPeerGradesGivenByDto : GetGradeBasesDto
    {
        public int ReviewerId { get; set; }
    }
}