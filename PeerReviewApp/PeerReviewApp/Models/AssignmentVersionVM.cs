namespace PeerReviewApp.Models
{
    public class AssignmentVersionVM
    {
        public IList<Assignment> Assignments { get; set; }
        public IList<Document> Documents { get; set; }
        public AssignmentVersion? AssnVersion { get; set; } = new AssignmentVersion();
        public int AssignmentId { get; set; }
        public int ReviewFormId { get; set; }
        public int InstructionsId { get; set; }
    }
}
