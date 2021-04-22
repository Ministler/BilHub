using System;
using System.Collections.Generic;

namespace BilHub.Core.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public Assignment AffiliatedAssignment { get; set; }
        public ProjectGroup AffiliatedGroup { get; set; }
        public Byte[] File { get; set; }
        public string Status { get; set; }
        public ICollection<Comment> MyProperty { get; set; }
    }
}