using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models;

namespace PeerReviewApp.Data
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IList<Assignment>> GetAssignmentsAsync()
        {
            var assignments = await _context.Assignments
            .Include(r => r.Course)
            .ToListAsync();

            return assignments;
        }

        public async Task<IList<Assignment>> GetAssignmentsByCourseAsync(int id)
        {
            var assignments = await _context.Assignments
            .Include(r => r.Course)
            .Where(r => r.Course.Id == id)
            .ToListAsync();

            return assignments;
        }
        public async Task<Assignment> GetAssignmentByIdAsync(int id)
        {

            var assignment = await _context.Assignments
            .Include(r => r.Course)
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

            return assignment;
        }
        public Task<int> AddAssignmentAsync(Assignment assignment)
        {

            throw new NotImplementedException();
        }
        public Task<int> UpdateAssignmentAsync(Assignment assignment)
        {

            throw new NotImplementedException();
        }
        public Task<int> DeleteAssignmentAsync(int id)
        {

            throw new NotImplementedException();
        }
    }
}
