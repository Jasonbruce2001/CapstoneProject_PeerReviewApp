namespace PeerReviewApp.Models
{
    public class Institution
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public IList<AppUser> Instructors { get; set; } = new List<AppUser>();
    }
}