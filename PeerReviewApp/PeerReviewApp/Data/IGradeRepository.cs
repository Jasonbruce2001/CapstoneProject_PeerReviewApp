using PeerReviewApp.Models;

namespace PeerReviewApp.Data
{
    public interface IGradeRepository
    {
        public Task<IList<Grade>> GetGradesAsync();
        public Task<IList<Grade>> GetGradesByStudentAsync(AppUser user);
        public Task<Grade> GetGradeByIdAsync(int id);
        public Task<int> AddGradeAsync(Grade model);
        public Task<int> UpdateGradeAsync(Grade model);
        public Task<int> DeleteGradeAsync(int id);
    }
}
