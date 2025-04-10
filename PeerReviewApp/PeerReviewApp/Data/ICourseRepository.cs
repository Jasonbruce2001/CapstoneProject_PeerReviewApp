using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface ICourseRepository
{
    public IList<Course> GetCourses();
    public Course GetCourse(int id);
    public Task<int> AddCourseAsync(Course course);
    public Task<int> UpdateCourseAsync(Course course);
    public Task<int> DeleteCourseAsync(int id);
}