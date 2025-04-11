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
        return await _context.Classes.ToListAsync();
    }

    public async Task<IList<Class>> GetClassesForStudentAsync(AppUser student)
    {
        var result = new List<Class>();
        var classes = await _context.Classes
                                            .Include(c => c.Students)
                                            .ToListAsync();

        foreach (var c in classes)
        {
            foreach (var s in c.Students)
            {
                if (s == student)
                {
                    result.Add(c);
                }
            }
        }
        
        return result;
    }

    public async Task<Class> GetClassByIdAsync(int classId)
    { 
        return await _context.Classes.FindAsync(classId)
               ?? throw new InvalidOperationException();
    }

    public async Task<int> AddClassAsync(Class newClass)
    {
        await _context.Classes.AddAsync(newClass);
        
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateClassAsync(Class updatedClass)
    {
        _context.Classes.Update(updatedClass);
        
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteClassAsync(int classId)
    {
        var classToRemove = await _context.Classes.FindAsync(classId);

        if (classToRemove != null)
        {
            _context.Classes.Remove(classToRemove);
        }
        
        return await _context.SaveChangesAsync();
    }
}