using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public class ClassRepository : IClassRepository
{
    private readonly ApplicationDbContext _context;

    public ClassRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IList<Class>> GetClassesAsync()
    {
        var classes = await _context.Classes
            .Include(r => r.Students)
            .ToListAsync();

        return classes;
    }

    public async Task<IList<Class>> GetArchivedClassesAsync()
    {
        var classes = await _context.Classes
            .Include(r => r.Students)
            .Where(r => r.IsArchived)
            .ToListAsync();

        return classes;
    }

    public async Task<IList<Class>> GetCurrentClassesAsync()
    {
        var classes = await _context.Classes
            .Include(r => r.Students)
            .Where(r => !r.IsArchived)
            .ToListAsync();

        return classes;
    }

    public Class GetClass(int classId)
    { 
        return _context.Classes.FirstOrDefault(c => c.ClassId == classId) 
               ?? throw new InvalidOperationException();
    }

    public async Task<int> AddClassAsync(Class newClass)
    {
        _context.Classes.Add(newClass);
        Task<int> task = _context.SaveChangesAsync();
        int result = await task;
        return result;
    }

    public Task<int> UpdateClassAsync(Class updatedClass)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteClassAsync(int classId)
    {
        throw new NotImplementedException();
    }
}