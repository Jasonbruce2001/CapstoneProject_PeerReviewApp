using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface IClassRepository
{
    public Task<IList<Class>> GetClassesAsync();
    public Task<IList<Class>> GetClassesAsync(string id);
    public Task<IList<Class>> GetClassesForStudentAsync(AppUser student);
    public Task<IList<Class>> GetClassesForInstructorAsync(AppUser instructor);
  
    public Task<IList<Course>> GetCoursesForInstructorAsync(AppUser instructor);
    public Task<Class> GetClassByIdAsync(int classId);
  
    public Task<IList<Class>> GetArchivedClassesAsync();
    public Task<IList<Class>> GetArchivedClassesAsync(string id);
    public Task<IList<Class>> GetCurrentClassesAsync(); //classes that are currently active
    public Task<IList<Class>> GetCurrentClassesAsync(string id);
    public Task<int> Archive(int id);
    //public Class GetClass(int classId);
  
    public Task<int> AddClassAsync(Class newClass);
    public Task<int> UpdateClassAsync(Class updatedClass);
    public Task<int> DeleteClassAsync(int classId);
    public Task<int> DeleteStudentFromClassAsync(int classId, string studentId);

    Task<int> AddStudentsToClassAsync(int classId, List<string> studentIds);

}