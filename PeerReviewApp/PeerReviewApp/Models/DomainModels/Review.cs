namespace PeerReviewApp.Models;

public class Review
{
    public int Id { get; set; }
    public Assignment Assignment { get; set; }
    public AppUser Reviewer { get; set; }
    public AppUser Reviewee { get; set; }
    DateTime DueDate { get; set; }
    String FilePath { get; set; }
}