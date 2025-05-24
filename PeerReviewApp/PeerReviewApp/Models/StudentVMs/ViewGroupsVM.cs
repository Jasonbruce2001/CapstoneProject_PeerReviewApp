namespace PeerReviewApp.Models
{
    public class ViewGroupsVM
    {
        public Class Class { get; set; }
        public IList<AppUser> Students { get; set; }
        public IList<Assignment> Assignments { get; set; }
    }
}
