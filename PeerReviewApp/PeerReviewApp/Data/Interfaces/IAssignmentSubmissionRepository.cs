using PeerReviewApp.Models;


namespace PeerReviewApp.Data;

public interface IAssignmentSubmissionRepository
{
    public Task<IList<AssignmentSubmission>> GetAllSubmissionsByStudentAsync(AppUser user);
    public Task<AssignmentSubmission> GetSubmissionByIdAsync(int id);
    public Task<IList<AssignmentSubmission>> GetSubmissionsByAssignmentAsync(int assignmentId);
    public Task<IList<AssignmentSubmission>> GetSubmissionsByReviewerAsync(AppUser user);
    public Task<int> AddAssignmentSubmissionAsync(AssignmentSubmission model);
    public Task<int> UpdateAssignmentSubmissionAsync(AssignmentSubmission model);
    public Task<int> DeleteAssignmentSubmissionAsync(AssignmentSubmission model);
    public Task<int> CheckForPartner(AssignmentSubmission model);
}