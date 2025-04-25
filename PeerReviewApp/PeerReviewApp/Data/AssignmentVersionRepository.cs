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
    }
}
