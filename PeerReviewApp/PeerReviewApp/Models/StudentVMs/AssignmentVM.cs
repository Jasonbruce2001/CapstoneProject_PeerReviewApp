namespace PeerReviewApp.Models;

public class AssignmentVM
{
    public IList<Assignment> Assignments { get; set; }
    public IList<AssignmentSubmission> Submissions { get; set; }
}