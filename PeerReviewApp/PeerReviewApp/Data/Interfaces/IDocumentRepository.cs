using PeerReviewApp.Models;

namespace PeerReviewApp.Data
{
    public interface IDocumentRepository
    {
        public Task<IList<Document>> GetDocumentsAsync();
        public Task<IList<Document>> GetDocumentsAsync(string id);
        public Task<Document> GetDocumentByIdAsync(int id);
        public Task<IList<Document>> GetDocumentsByUserAsync(AppUser user);
        public Task<int> AddDocumentAsync(Document model);
        public Task<int> UpdateDocumentAsync(Document model);
        public Task<int> DeleteDocumentAsync(int id);
    }
}
