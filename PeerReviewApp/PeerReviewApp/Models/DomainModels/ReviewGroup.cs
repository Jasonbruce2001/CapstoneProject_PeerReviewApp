namespace PeerReviewApp.Models;

public class ReviewGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<Review> Reviews { get; set; } = new List<Review>();
    public IList<AppUser> Students { get; set; } = new List<AppUser>();
}

