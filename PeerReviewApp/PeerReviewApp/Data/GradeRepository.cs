using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models;

namespace PeerReviewApp.Data
{
    public class GradeRepository : IGradeRepository
    {

        private readonly ApplicationDbContext _context;

        public GradeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IList<Grade>> GetGradesAsync()
        {
            var grades = await _context.Grades
            .Include(r => r.Assignment)
            .ToListAsync();

            return grades;
        }
        public async Task<IList<Grade>> GetGradesByStudentAsync(AppUser user)
        {
            var grades = await _context.Grades
            .Include(r => r.Assignment)
            .ThenInclude(r => r.Course)
            .Where(r => r.Student == user)
            .OrderBy(r => r.Assignment.Course)
            .ToListAsync();

            return grades;
        }
        public Task<Grade> GetGradeByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> AddGradeAsync(Grade model)
        {
            throw new NotImplementedException();
        }
        public Task<int> UpdateGradeAsync(Grade model)
        {
            throw new NotImplementedException();
        }
        public Task<int> DeleteGradeAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
