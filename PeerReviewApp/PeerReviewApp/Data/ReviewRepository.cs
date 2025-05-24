using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models;

namespace PeerReviewApp.Data
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Review>> GetReviewsAsync()
        {
            var review = await _context.Reviews
                .Include(r => r.Reviewee)
                .Include(r => r.Reviewer)
                .Include(r => r.ReviewDocument)
                .ToListAsync();

            return review;
        }
        public async Task<IList<Review>> GetReviewsByReviewerAsync(AppUser user)
        {
            var review = await _context.Reviews
                .Include(r => r.Reviewee)
                .Include(r => r.Reviewer)
                .Include(r => r.ReviewDocument)
                .Where(r => r.Reviewer == user)
                .ToListAsync();

            return review;
        }
        public async Task<IList<Review>> GetReviewsByRevieweeAsync(AppUser user)
        {
            var review = await _context.Reviews
                .Include(r => r.Reviewee)
                .Include(r => r.Reviewer)
                .Include(r => r.ReviewDocument)
                .Where(r => r.Reviewee == user)
                .ToListAsync();

            return review;
        }
        public async Task<Review> GetReviewByIdAsync(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Reviewee)
                .Include(r => r.Reviewer)
                .Include(r => r.ReviewDocument)
                .Where(r => r.Id == id)
                .SingleOrDefaultAsync();

            return review;
        }
        public Task<int> AddReviewAsync(Review model)
        {
            throw new NotImplementedException();
        }
        public async Task<int> UpdateReviewAsync(Review model)
        {

            _context.Reviews.Update(model);

            return await _context.SaveChangesAsync();
        }
        public Task<int> DeleteReviewAsync(int id)
        {
            throw new NotImplementedException();
        }


    }
}