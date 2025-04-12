namespace PeerReviewApp.Models;

public class Class
{
    public int ClassId { get; set; }
    public string term { get; set; }
    
    //foreign keys
    public AppUser Instructor { get; set; }
    public Course ParentCourse { get; set; }
    public IList<AppUser> Students { get; set; } = new List<AppUser>();
}