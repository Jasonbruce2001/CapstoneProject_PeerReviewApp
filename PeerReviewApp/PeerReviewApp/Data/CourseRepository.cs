using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public IList<Course> GetCourses()
    {
        throw new NotImplementedException();
    }

    public Course GetCourse(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> AddCourseAsync(Course course)
    {
        throw new NotImplementedException();
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