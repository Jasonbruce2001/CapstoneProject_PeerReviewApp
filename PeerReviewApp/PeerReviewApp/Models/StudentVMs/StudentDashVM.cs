namespace PeerReviewApp.Models;

public class StudentDashVM
{
    public AppUser User { get; set; }
    public IList<Class> Classes { get; set; }
    public IList<ReviewGroup> ReviewGroups { get; set; }
    public IList<Document> Documents { get; set; }
    public IList<AssignmentSubmission> ReviewsToDo { get; set; }
    public IList<Review> ReviewsReceived { get; set; }
}