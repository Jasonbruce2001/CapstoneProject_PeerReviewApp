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
            .ToListAsync();

            return grades;
        }
        public async Task<IList<Grade>> GetGradesByStudentAsync(AppUser user)
        {
            var grades = await _context.Grades
            .Where(r => r.Student == user)
            .ToListAsync();

            return grades;
        }
        public Task<Grade> GetGradeByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<int> AddGradeAsync(Grade model)
        {
            _context.Grades.Add(model);
            
            return await _context.SaveChangesAsync();
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
