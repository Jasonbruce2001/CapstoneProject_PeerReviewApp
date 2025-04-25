using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public class ReviewGroupRepository : IReviewGroupRepository
{
    private readonly ApplicationDbContext _context;
    
    public ReviewGroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ReviewGroup>> GetAllReviewGroupsForInstructorAsync(string userId)
    {
        return await _context.ReviewGroups
                            .Include(group => group.Reviews)
                            .Include(group => group.Students)
                            .ToListAsync();
    }

    public async Task<ReviewGroup> GetReviewGroupByIdAsync(int id)
    {
        return await _context.ReviewGroups
            .Include(group => group.Students)
            .Include(group => group.Reviews)
            .Where(group => group.Id == id)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }

    public async Task<int> AddReviewGroupAsync(ReviewGroup reviewGroup)
    {
        _context.ReviewGroups.Add(reviewGroup);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateReviewGroupAsync(ReviewGroup reviewGroup)
    {
        _context.ReviewGroups.Update(reviewGroup);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteReviewGroupByIdAsync(int id)
    {
        var group = _context.ReviewGroups.Find(id);
        
        if (group != null)
        {
            _context.ReviewGroups.Remove(group);
        }
        
        return await _context.SaveChangesAsync();
    }
    
    //specialized methods
    public async Task<int> AddStudentToGroupAsync(ReviewGroup group, AppUser student)
    {
        group.Students.Add(student);
        
        _context.ReviewGroups.Update(group);
        
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteStudentFromGroupAsync(ReviewGroup group, AppUser student)
    {
        group.Students.Remove(student);
        
        _context.ReviewGroups.Update(group);
        
        return await _context.SaveChangesAsync();
    }
}