namespace PeerReviewApp.Models;

public class InstructorDashVM
{
    public AppUser Instructor { get; set; }
    public IList<Course> Courses { get; set; }
    public IList<Class> Classes { get; set; }
}