using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
 
namespace backend.Dtos.PeerGradeAssignment
{
    public class PeerGradeAssignmentInfoDto
    {   
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int MaxGrade { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime LastEdited { get; set; }
    }
}