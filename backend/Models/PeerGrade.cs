namespace backend.Models
{
    public class PeerGrade : GradeBase
    {
        public Section AffiliatedSection { get; set; }
        public int AffiliatedSectionID { get; set; }
        public int ReviewerId { get; set; }
        public int RevieweeId { get; set; }
    }
}