namespace PeerReviewApp.Models;

public class AssignmentSubmission
{
    public int Id { get; set; }
    public string AssignmentLink { get; set; }
    public AssignmentVersion AssignmentVersion { get; set; }
    public DateTime SubmissionDate { get; set; }
    public Review Review { get; set; }
    public AppUser Submitter { get; set; }
}