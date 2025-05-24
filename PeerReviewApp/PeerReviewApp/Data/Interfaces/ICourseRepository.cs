using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface ICourseRepository
{
    public Task<IList<Course>> GetCoursesAsync();
    public Task<Course> GetCourseByIdAsync(int id);
    public Task<int> AddCourseAsync(Course course);
    public Task<int> UpdateCourseAsync(Course course);
    public Task<int> DeleteCourseAsync(int id);
}