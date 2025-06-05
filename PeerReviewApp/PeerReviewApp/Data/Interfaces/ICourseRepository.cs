using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface ICourseRepository
{
    public Task<IList<Course>> GetCoursesAsync();
    public Task<IList<Course>> GetCoursesAsync(AppUser instructor);
    public Task<Course> GetCourseByIdAsync(int id);
    public Task<IList<Course>> GetCoursesByInstructorAsync(string id);
    public Task<IList<Course>> SearchByNameAsync(string name);
    public Task<IList<Course>> SearchByInstitutionAsync(string name, int institutionId);
    public Task<int> AddCourseAsync(Course course);
    public Task<int> UpdateCourseAsync(Course course);
    public Task<int> DeleteCourseAsync(int id);
}