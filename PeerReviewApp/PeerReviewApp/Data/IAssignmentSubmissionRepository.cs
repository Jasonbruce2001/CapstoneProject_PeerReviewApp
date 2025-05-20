using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface IAssignmentSubmissionRepository
{
    public Task<IList<AssignmentSubmission>> GetAllSubmissionsByStudentAsync(AppUser user);
    public Task<int> AddAssignmentSubmissionAsync(AssignmentSubmission model);
    public Task<int> UpdateAssignmentSubmissionAsync(AssignmentSubmission model);
    public Task<int> DeleteAssignmentSubmissionAsync(AssignmentSubmission model);
}