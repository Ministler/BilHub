namespace backend.Dtos.Comment.FeedbackItems
{
    public class NewFeedbacksDto
    {
        public UserFeedbackDto user { get; set; }
        public FeedbackItemDto feedback { get; set; }
        public FeedbackCourseItem course { get; set; }
        public FeedbackSubmissionItem submission { get; set; }
    }
}