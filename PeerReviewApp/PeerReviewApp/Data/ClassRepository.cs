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


    //overloaded for getting classes by instructor by id
    public async Task<IList<Class>> GetClassesAsync(string id)
    {
        var classes = await _context.Classes
            .Include(r => r.Students)
            .Include(r => r.Instructor)
            .Include(r => r.ParentCourse)
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

    public async Task<IList<Class>> GetClassesForStudentAsync(AppUser student)
    {
        var result = new List<Class>();
        var classes = await _context.Classes
                                            .Include(c => c.Students)
                                            .Include(c => c.ParentCourse)
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
    
    public async Task<IList<Class>> GetClassesForInstructorAsync(AppUser instructor)
    {
        var result = new List<Class>();
        var classes = await _context.Classes
            .Include(c => c.Students)
            .Include(c => c.ParentCourse)
            .ToListAsync();

        foreach (var c in classes)
        {
            if (c.Instructor == instructor)
            {
                result.Add(c);
            }
        }
        
        return result;
    }

    public async Task<IList<Course>> GetCoursesForInstructorAsync(AppUser instructor)
    {
        var result = new List<Course>();
        var allClasses = await _context.Classes
                                            .Include(c => c.Instructor)
                                            .Include(c => c.ParentCourse).ToListAsync();

        foreach(Class c in allClasses)
        {
            if (c.Instructor == instructor)
            {
                result.Add(c.ParentCourse);
            }
        }
        
        return result;
    }

    public async Task<Class> GetClassByIdAsync(int classId)
    {
        return await _context.Classes
            .Include(c => c.Students)
            .Include(c => c.ParentCourse)
            .ThenInclude(c => c.Assignments)
            .ThenInclude(c => c.Versions)
            .ThenInclude(c => c.Students)
            .FirstOrDefaultAsync(c => c.ClassId == classId);
               
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

    public async Task<int> DeleteStudentFromClassAsync(int classId, string studentId)
    {
        var cls = await _context.Classes
            .Include(r => r.Students)
            .FirstOrDefaultAsync(c => c.ClassId == classId);

        cls.Students.Remove(cls.Students.Where(r => r.Id == studentId).FirstOrDefault());

        Task<int> task =_context.SaveChangesAsync();
        int result = await task;

        return result;
    }
    
    
}