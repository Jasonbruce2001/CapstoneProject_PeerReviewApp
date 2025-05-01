using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models;

namespace PeerReviewApp.Data
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IList<Document>> GetDocumentsAsync()
        {


            var documents = await _context.Documents
            .Include(r => r.Uploader)
            .ToListAsync();

            return documents;
        }
        public async Task<IList<Document>> GetDocumentsAsync(string id)
        {


            var documents = await _context.Documents
            .Include(r => r.Uploader)
            .Where(r => r.Uploader.Id == id)
            .ToListAsync();

            return documents;
        }
        public async Task<Document> GetDocumentByIdAsync(int id)
        {
            var document = await _context.Documents
                .Include(r => r.Uploader)
                .FirstOrDefaultAsync(r => r.Id == id);

            return document;
        }

        public async Task<IList<Document>> GetDocumentsByUserAsync(AppUser user)
        {
            var documents = await _context.Documents
                .Include(d => d.Uploader)
                .Where(d => d.Uploader.Id == user.Id)
                .ToListAsync();
                
            return documents;
        }
        
        public Task<int> AddDocumentAsync(Document model)
        {

            throw new NotImplementedException();
        }
        public Task<int> UpdateDocumentAsync(Document model)
        {

            throw new NotImplementedException();
        }
        public Task<int> DeleteDocumentAsync(int id)
        {

            throw new NotImplementedException();
        }
    }
}
