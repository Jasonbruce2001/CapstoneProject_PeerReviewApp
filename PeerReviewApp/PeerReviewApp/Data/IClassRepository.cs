using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface IClassRepository
{
    public Task<IList<Class>> GetClassesAsync();
    public Task<IList<Class>> GetClassesForStudentAsync(AppUser student);
    public Task<Class> GetClassByIdAsync(int classId);
    public Task<int> AddClassAsync(Class newClass);
    public Task<int> UpdateClassAsync(Class updatedClass);
    public Task<int> DeleteClassAsync(int classId);
}