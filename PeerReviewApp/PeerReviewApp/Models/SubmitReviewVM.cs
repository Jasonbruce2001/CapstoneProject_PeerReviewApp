namespace PeerReviewApp.Models
{
    public class SubmitReviewVM
    {
        public IList<Document> Documents { get; set; }
        public int ReviewId { get; set; }
        public int DocumentId { get; set; }
    }
}
