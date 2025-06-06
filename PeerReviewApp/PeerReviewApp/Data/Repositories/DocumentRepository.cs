using Microsoft.AspNetCore.Mvc.RazorPages;
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
        
        public async Task<int> AddDocumentAsync(Document model)
        {
            _context.Documents.Add(model);
            
            return await _context.SaveChangesAsync();
        }
        public Task<int> UpdateDocumentAsync(Document model)
        {
            throw new NotImplementedException();
        }
        public async Task<int> DeleteDocumentAsync(int id)
        {
            var doc = await _context.Documents.FindAsync(id);
            
            _context.Documents.Remove(doc);

            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            
            return 0;
        }
    }
}
