using System.Collections.Generic;

namespace backend.Dtos.ProjectGroup
{
    public class OzgurSubmissionDto
    {
        public string SubmissionName { get; set; }
        public List<decimal> Grades { get; set; }
    }
}