using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
 
namespace backend.Dtos.GradeBase
{
    public class GradeBaseInfoDto
    {
        public int Id { get; set; }
        public decimal MaxGrade { get; set; }
        public decimal Grade { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        //public string FilePath { get; set; }
    }
}