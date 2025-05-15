namespace PeerReviewApp.Models.ViewModels;

public class DetailedAssignmentVM
{ 
    public AssignmentVersion Assignment { get; set; }
    public string AssignmentLink { get; set; }
    public bool HasSubmitted { get; set; }
    public AssignmentSubmission? Submission { get; set; }
}