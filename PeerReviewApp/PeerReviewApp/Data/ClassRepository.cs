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
            .Include(r => r.ParentCourse)
            .Include(r => r.Instructor)
            .ToListAsync();

        return classes;
    }
    public async Task<IList<Class>> GetClassesAsync(string id)
    {
        var classes = await _context.Classes
            .Include(r => r.Students)
            .Include(r => r.ParentCourse)
            .Include(r => r.Instructor)
            .Where (r => r.Instructor.Id == id)
            .OrderBy(r => r.IsArchived)
            .ToListAsync();

        return classes;
    }

    public async Task<IList<Class>> GetArchivedClassesAsync()
    {
        var classes = await _context.Classes
            .Include(r => r.Students)
            .Include(r => r.ParentCourse)
            .Include(r => r.Instructor)
            .Where(r => r.IsArchived)
            .ToListAsync();

        return classes;
    }
    public async Task<IList<Class>> GetArchivedClassesAsync(string id)
    {
        var classes = await _context.Classes
            .Include(r => r.Students)
            .Include(r => r.ParentCourse)
            .Include(r => r.Instructor)
            .Where(r => r.IsArchived)
            .Where(r => r.Instructor.Id == id)
            .ToListAsync();

        return classes;
    }

    public async Task<IList<Class>> GetCurrentClassesAsync()
    {
        var classes = await _context.Classes
            .Include(r => r.Students)
            .Include(r => r.ParentCourse)
            .Include(r => r.Instructor)
            .Where(r => !r.IsArchived)
            .ToListAsync();

        return classes;
    }



    public async Task<IList<Class>> GetCurrentClassesAsync(string id)
    {
        var classes = await _context.Classes
            .Include(r => r.Students)
            .Include(r => r.ParentCourse)
            .Include(r => r.Instructor)
            .Where(r => !r.IsArchived)
            .Where(r => r.Instructor.Id == id)
            .ToListAsync();

        return classes;
    }


    public async Task<int> Archive(int id)
    {

        var cls = _context.Classes.FirstOrDefault(c => c.ClassId == id);
        cls.IsArchived = !cls.IsArchived;
        Task<int> task = _context.SaveChangesAsync();
        int result = await task;
        return result;
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