using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IList<Course>> GetCoursesAsync()
    {
        return await _context.Courses
                            .Include(c => c.Institution)
                            .ToListAsync();
    }                            

    public async Task<int> AddCourseAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        
        return await _context.SaveChangesAsync();
    }

    public async Task<Course> GetCourseByIdAsync(int id)
    {
        var course = await _context.Courses
            .Include(r => r.Institution)
            .FirstOrDefaultAsync(r => r.Id == id);

        return course;
    }

    public async Task<int> AddCourseAsync(Course course)
    {
        _context.Courses.Add(course);
        
        Task<int> task = _context.SaveChangesAsync();
        int result = await task;
        
        return result;
    }

    public async Task<int> UpdateCourseAsync(Course course)
    {
        _context.Courses.Update(course);
        
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteCourseAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        
        if (course != null)
        {
            _context.Courses.Remove(course);
        }
        
        return await _context.SaveChangesAsync();
    }
}