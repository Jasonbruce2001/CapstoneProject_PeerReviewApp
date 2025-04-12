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
        return  await _context.Courses
            .Include(r => r.Institution)
            .ToListAsync();
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

    public Task<int> UpdateCourseAsync(Course course)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteCourseAsync(int id)
    {
        throw new NotImplementedException();
    }
}