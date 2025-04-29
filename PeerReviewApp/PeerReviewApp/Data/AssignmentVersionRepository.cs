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
        public Task<AssignmentVersion> GetAssignmentVersionByIdAsync(int id)
        {

            throw new NotImplementedException();
        }
        public async Task<int> AddAssignmentVersionAsync(AssignmentVersion model)
        {
            await _context.AssignmentVersions.AddAsync(model);

            return await _context.SaveChangesAsync();
        }
        public Task<int> UpdateAssignmentVersionAsync(AssignmentVersion model)
        {

            throw new NotImplementedException();
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

            throw new NotImplementedException();
        }
    }
}
