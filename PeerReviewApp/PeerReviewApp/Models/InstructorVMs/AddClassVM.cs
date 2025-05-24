namespace PeerReviewApp.Models
{
    public class AddClassVM
    {
        public IList<Course> Courses { get; set; } = new List<Course>();
        public Class Class { get; set; } = new Class();
        public int CourseId { get; set; }
    }
}
