using PeerReviewApp.Models;
namespace PeerReviewApp.Data
{
    public interface IAssignmentVersionRepository
    {
        public Task<IList<AssignmentVersion>> GetAssignmentVersionsAsync();
        public Task<AssignmentVersion> GetAssignmentVersionByIdAsync(int id);
        public Task<IList<AssignmentVersion>> GetAssignmentVersionsForStudentAsync(AppUser user);
        public Task<IList<AssignmentVersion>> GetAssignmentVersionsForAssignmentAsync(int assignmentId);
        public Task<int> AddAssignmentVersionAsync(AssignmentVersion model);
        public Task<int> UpdateAssignmentVersionAsync(AssignmentVersion model);
        public Task<int> DeleteAssignmentVersionAsync(int id);
        public Task<int> DeleteStudentsFromAssignmentVersionAsync(IList<AppUser> students, int assignmentId);
        public Task<int> DeleteStudentFromAssignmentVersionAsync(string studentId, int assignmentId);
        public Task<int> AddStudentsToAssignmentVersionsAsync(IList<AppUser> students, int assignmentId);
    }
}
