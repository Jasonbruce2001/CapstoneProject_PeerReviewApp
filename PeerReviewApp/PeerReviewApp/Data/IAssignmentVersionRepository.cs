using PeerReviewApp.Models;
namespace PeerReviewApp.Data
{
    public interface IAssignmentVersionRepository
    {
        public Task<IList<AssignmentVersion>> GetAssignmentVersionsAsync();
        public Task<AssignmentVersion> GetAssignmentVersionByIdAsync(int id);
        public Task<int> AddAssignmentVersionAsync(AssignmentVersion model);
        public Task<int> UpdateAssignmentVersionAsync(AssignmentVersion model);
        public Task<int> DeleteAssignmentVersionAsync(int id);
    }
}
