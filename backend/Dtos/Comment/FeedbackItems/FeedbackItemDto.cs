using System;

namespace backend.Dtos.Comment.FeedbackItems
{
    public class FeedbackItemDto
    {
        public string caption { get; set; }
        public DateTime date { get; set; }
        public int commentId { get; set; }
        public decimal grade { get; set; }

    }
}