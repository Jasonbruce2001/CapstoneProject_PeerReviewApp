using System.ComponentModel.DataAnnotations.Schema;

namespace PeerReviewApp.Models
{
    public class Document
    {
        [NotMapped]
        public IFormFile? File { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public DateTime DateUploaded { get; set; }
        public AppUser Uploader { get; set; }
    }
}
