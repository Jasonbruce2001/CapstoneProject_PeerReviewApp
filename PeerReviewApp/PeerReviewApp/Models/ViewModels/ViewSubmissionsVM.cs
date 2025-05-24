namespace PeerReviewApp.Models.ViewModels;

public class ViewSubmissionsVM
{
    public IList<AssignmentSubmission> Submissions { get; set; }
    public Grade Grade { get; set; } = new Grade();
}