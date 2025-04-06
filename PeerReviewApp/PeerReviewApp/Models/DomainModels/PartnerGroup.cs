namespace PeerReviewApp.Models
{
    public class PartnerGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Course Course { get; set; }
        public IList<AppUser> Student { get; set; } = [];
    }
}
