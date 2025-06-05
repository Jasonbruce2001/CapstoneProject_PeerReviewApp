namespace PeerReviewApp.Models
{
    public class ViewGradesVM
    {
        public IList<AssignmentSubmission> Submissions { get; set; }
        public IList<AssignmentSubmission> Reviews { get; set; }
    }
}
