namespace PeerReviewApp.Models
{
    public class ReadyForGradingVM
    {
        public List<UnGradedSubmissionGroup> UnGradedSubmissions { get; set; } = new();
        public List<UnGradedReviewGroup> UnGradedReviews { get; set; } = new();
        public int TotalItemsToGrade { get; set; }
    }

    public class UnGradedSubmissionGroup
    {
        public Assignment Assignment { get; set; }
        public Class Class { get; set; }
        public List<AssignmentSubmission> Submissions { get; set; } = new();
    }

    public class UnGradedReviewGroup
    {
        public Assignment Assignment { get; set; }
        public Class Class { get; set; }
        public List<Review> Reviews { get; set; } = new();
    }
}