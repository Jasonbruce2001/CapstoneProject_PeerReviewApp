using PeerReviewApp.Models;

namespace PeerReviewApp.Data
{
    public interface IAssignmentRepository
    {
        public Task<IList<Assignment>> GetAssignmentsAsync();
        public Task<IList<Assignment>> GetAssignmentsByCourseAsync(int courseId);
        public Task<IList<Assignment>> GetStudentAssignments(AppUser student);
        public Task<Assignment> GetAssignmentByIdAsync(int id);
        public Task<int> AddAssignmentAsync(Assignment assignment);
        public Task<int> UpdateAssignmentAsync(Assignment assignment);
        public Task<int> DeleteAssignmentAsync(int id);
    }
}
