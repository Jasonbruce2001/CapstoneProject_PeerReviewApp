using System.ComponentModel.DataAnnotations;

namespace PeerReviewApp.Models
{
    public class AssignmentVersionVM
    {
        public IList<Assignment> Assignments { get; set; }
        public IList<Document> Documents { get; set; }
        public AssignmentVersion? AssnVersion { get; set; } = new AssignmentVersion();
        public int AssignmentId { get; set; }
        [Required(ErrorMessage = "You must select a review form.")]
        public int ReviewFormId { get; set; }
        [Required(ErrorMessage = "You must select a instructions form.")]
        public int InstructionsId { get; set; }
    }
}
