namespace backend.Models
{
    public class GradeBase
    {
        public int Id { get; set; }
        public double MaxGrade { get; set; }
        public double Grade { get; set; }
        public string AdditionalComment { get; set; }
    }
}