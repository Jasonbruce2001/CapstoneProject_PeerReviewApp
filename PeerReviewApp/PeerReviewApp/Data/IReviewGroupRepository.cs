using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface IReviewGroupRepository
{
    public Task<List<ReviewGroup>> GetAllReviewGroupsForInstructorAsync(string userId);
    public Task<ReviewGroup> GetReviewGroupByIdAsync(int id);
    public Task<int> AddReviewGroupAsync(ReviewGroup reviewGroup);
    public Task<int> UpdateReviewGroupAsync(ReviewGroup reviewGroup);
    public Task<int> DeleteReviewGroupByIdAsync(int id);

  
}