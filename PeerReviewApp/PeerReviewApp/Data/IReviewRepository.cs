using PeerReviewApp.Models;

namespace PeerReviewApp.Data
{
    public interface IReviewRepository
    {
        public Task<IList<Review>> GetReviewsAsync();
        public Task<IList<Review>> GetReviewsByReviewerAsync(AppUser user);
        public Task<IList<Review>> GetReviewsByRevieweeAsync(AppUser user);
        public Task<Review> GetReviewByIdAsync(int id);
        public Task<int> AddReviewAsync(Review model);
        public Task<int> UpdateReviewAsync(Review model);
        public Task<int> DeleteReviewAsync(int id);

      
    }
}
