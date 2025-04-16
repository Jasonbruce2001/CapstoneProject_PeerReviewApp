namespace PeerReviewApp.Models
{
    public class AddCourseVM
    {
        public IList<Institution> Institutions { get; set; } = new List<Institution>();
        public Course Course { get; set; }
        public int InstId { get; set; }
    }
}
