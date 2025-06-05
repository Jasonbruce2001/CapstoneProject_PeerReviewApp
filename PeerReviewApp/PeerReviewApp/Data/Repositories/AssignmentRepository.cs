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
            return await _context.Assignments
                .Include(a => a.Course)
                .ToListAsync();
        }

        public async Task<IList<Assignment>> GetAssignmentsByInstructorAsync(AppUser instructor)
        {
            var mostAssignments = await _context.Assignments
                .Include(a => a.Course)
                .ThenInclude(c => c.Subclasses)
                .ThenInclude(sc => sc.Instructor)
                .ToListAsync();

            IList<Assignment> assignments = new List<Assignment>();
            foreach (var assignment in mostAssignments)
            {
                foreach(var cls in assignment.Course.Subclasses)
                {
                    if (cls.Instructor == instructor)
                    {
                        assignments.Add(assignment);
                    }
                }
            }

            return assignments;
        }
        public Task<IList<Assignment>> GetStudentAssignments(AppUser student)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Assignment>> GetAssignmentsByCourseAsync(int courseId)
        { 
            return await _context.Assignments
                .Include(a => a.Course)
                .Include(a => a.Versions)
                .Where(a => a.Course.Id == courseId)
                .ToListAsync();
        }

        public async Task<Assignment> GetAssignmentByIdAsync(int id)
        { 
            return await _context.Assignments
                .Include(a => a.Course)
                .Include(a => a.Versions)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<int> AddAssignmentAsync(Assignment assignment)
        {
            await _context.Assignments.AddAsync(assignment);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAssignmentAsync(Assignment assignment)
        {
            _context.Assignments.Update(assignment);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAssignmentAsync(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
            }
            return await _context.SaveChangesAsync();

        }
    }
}
