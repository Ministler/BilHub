using System.Collections.Generic;

namespace backend.Dtos.Comment.FeedbackItems
{
    public class FeedbacksDto
    {
        public List<NewFeedbacksDto> InstructorComments { get; set; }
        public List<NewFeedbacksDto> TAFeedbacks { get; set; }
        public List<NewFeedbacksDto> StudentsFeedbacks { get; set; }
    }
}