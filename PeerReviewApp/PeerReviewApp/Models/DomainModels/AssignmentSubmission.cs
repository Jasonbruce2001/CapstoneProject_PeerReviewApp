namespace PeerReviewApp.Models;

public class AssignmentSubmission
{
    public int Id;
    public string AssignmentLink;
    public DateTime SubmissionDate;
    public Review Review;
    public AppUser Submitter;
}