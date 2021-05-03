using System;

namespace backend.Models
{
    public class GradeBase
    {
        public int Id { get; set; }
        public decimal MaxGrade { get; set; }
        public decimal Grade { get; set; }
        public string Comment { get; set; }
        public DateTime LastEdited { get; set; }
    }
}