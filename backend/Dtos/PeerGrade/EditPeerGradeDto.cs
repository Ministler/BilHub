using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using backend.Dtos.GradeBase;
 
namespace backend.Dtos.PeerGrade
{
    public class EditPeerGradeDto
    {
        public int Id {get; set; }
        public decimal Grade { get; set; }
        public string Comment { get; set; }
        public DateTime LastEdited { get; set; }
    }
}