using System.ComponentModel.DataAnnotations;

namespace PeerReviewApp.Models.ViewModels;

public class ViewSubmissionsVM
{
    public IList<AssignmentSubmission> Submissions { get; set; }
    [Range(0, 100)]
    public Grade Grade { get; set; } = new Grade();
}