namespace PeerReviewApp.Models;

public class Review
{
    public int Id { get; set; }
    public AppUser Reviewer { get; set; }
    public AppUser Reviewee { get; set; }
    public Document ReviewDocument { get; set; }
}