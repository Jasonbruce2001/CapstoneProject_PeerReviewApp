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
                            .Include(c => c.Assignments)
                            .Include(c => c.Subclasses)
                            .ToListAsync();
    }
    public async Task<IList<Course>> GetCoursesAsync(AppUser instructor)
    {
        return await _context.Courses
                            .Include(c => c.Institution)
                            .Include(c => c.Assignments)
                            .Include(c => c.Subclasses)
                            .Where(c => c.Institution.Instructors.Contains(instructor))
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
            .Include(c => c.Assignments)
            .Include(c => c.Subclasses)
            .FirstOrDefaultAsync(r => r.Id == id);

        return course;
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