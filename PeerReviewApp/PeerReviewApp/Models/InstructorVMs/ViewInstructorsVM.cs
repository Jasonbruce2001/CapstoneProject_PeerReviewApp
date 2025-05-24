namespace PeerReviewApp.Models;

public class ViewInstructorsVM
{ 
    public IList<AppUser> allInstructors { get; set; }
    public Institution institution { get; set; }
}       