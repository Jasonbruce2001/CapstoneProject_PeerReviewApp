using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models;

namespace PeerReviewApp.Data
{
    public class AssignmentVersionRepository : IAssignmentVersionRepository
    {
        private readonly ApplicationDbContext _context;

        public AssignmentVersionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<IList<AssignmentVersion>> GetAssignmentVersionsAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<AssignmentVersion> GetAssignmentVersionByIdAsync(int id)
        {
            return await _context.AssignmentVersions
                .Include(a => a.Instructions)
                .Include(a => a.Submissions)
                .ThenInclude(s => s.Review)
                .ThenInclude(r => r.ReviewDocument)
                .Include(a => a.ReviewForm)
                .Include(a => a.ParentAssignment)
                .Include(a => a.Students)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IList<AssignmentVersion>> GetAssignmentVersionsForStudentAsync(AppUser user)
        {
            var allAssignments = await _context.AssignmentVersions
                                                                    .Include(a => a.Students)
                                                                    .Include(a => a.Instructions)
                                                                    .Include(a => a.ReviewForm)
                                                                    .Include(a => a.Submissions)
                                                                    .ThenInclude(s => s.Review)
                                                                    .ThenInclude(r => r.ReviewDocument)
                                                                    .Include(a => a.ParentAssignment)
                                                                    .ThenInclude(p => p.Course)
                                                                    .ToListAsync();
            var assignments = new List<AssignmentVersion>();

            foreach (var a in allAssignments)
            {
                if (a.Students.Contains(user))
                {
                    assignments.Add(a);
                }
            }
            
            return assignments;
        }

        public async Task<IList<AssignmentVersion>> GetAssignmentVersionsForAssignmentAsync(int assignmentId)
        {
            return await _context.AssignmentVersions.Where(v => v.ParentAssignment.Id == assignmentId).ToListAsync();
        }

        public async Task<int> AddAssignmentVersionAsync(AssignmentVersion model)
        {
            await _context.AssignmentVersions.AddAsync(model);

            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateAssignmentVersionAsync(AssignmentVersion model)
        {
            _context.AssignmentVersions.Update(model);
            
            return await _context.SaveChangesAsync();
        }
        public Task<int> DeleteAssignmentVersionAsync(int id)
        {

            throw new NotImplementedException();
        }
        public async Task<int> DeleteStudentsFromAssignmentVersionAsync(IList<AppUser> students, int assignmentId)
        {

            var asnVersions = await _context.AssignmentVersions
                .Include(r => r.Students)
                .Where(r => r.ParentAssignment.Id == assignmentId)
                .ToListAsync();

            foreach (var asnVersion in asnVersions) 
            {
                foreach (var student in students)
                {
                    asnVersion.Students.Remove(student);
                }
            }

            Task<int> task = _context.SaveChangesAsync();
            int result = await task;

            return result;
        }
        public async Task<int> DeleteStudentFromAssignmentVersionAsync(string studentId, int assignmentId)
        {
            var asnVersions = await _context.AssignmentVersions
                .Include(r => r.Students)
                .Where(r => r.ParentAssignment.Id == assignmentId)
                .ToListAsync();


            foreach (var asnVersion in asnVersions)
            {
                foreach (var student in asnVersion.Students.ToList())
                {
                    if (student.Id == studentId)
                    {
                        asnVersion.Students.Remove(student);
                    }
                }
            }

            Task<int> task = _context.SaveChangesAsync();
            int result = await task;

            return result;
        }
        public async Task<int> AddStudentsToAssignmentVersionsAsync(IList<AppUser> students, int assignmentId)
        {
            var asnVersions = await _context.AssignmentVersions
                .Include(r => r.Students)
                .Where(r => r.ParentAssignment.Id == assignmentId)
                .ToListAsync();

            int count = students.Count/asnVersions.Count-1;
            int j = 0;
            foreach (var asnVersion in asnVersions)
            {
                for (int i = j; i < students.Count; i=i+count)
                {
                    asnVersion.Students.Add(students[i]);

                }
                j = j + 1;
            }

            Task<int> task = _context.SaveChangesAsync();
            int result = await task;

            return result;
        }
    }
}
