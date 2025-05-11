namespace PeerReviewApp.Models;

public class AssignmentVersion
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string TextInstructions { get; set; }
    public Document Instructions { get; set; }
    public Document ReviewForm { get; set; }
    public IList<AppUser> Students { get; set; } = new List<AppUser>();
    public IList<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
    public Assignment ParentAssignment { get; set; }
}