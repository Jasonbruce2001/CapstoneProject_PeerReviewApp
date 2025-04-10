using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public class ClassRepository : IClassRepository
{
    private readonly ApplicationDbContext _context;

    public ClassRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IList<Class> GetClasses()
    {
        return _context.Classes.ToList();
    }

    public Class GetClass(int classId)
    { 
        return _context.Classes.FirstOrDefault(c => c.ClassId == classId) 
               ?? throw new InvalidOperationException();
    }

    public Task<int> AddClassAsync(Class newClass)
    {
        throw new NotImplementedException();
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