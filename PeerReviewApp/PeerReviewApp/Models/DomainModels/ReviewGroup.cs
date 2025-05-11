namespace PeerReviewApp.Models;

public class ReviewGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<AppUser> Students { get; set; } = new List<AppUser>();
}

