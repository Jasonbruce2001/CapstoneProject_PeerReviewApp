using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface IClassRepository
{
    public IList<Class> GetClasses();
    public IList<Class> GetClassesForStudent(AppUser student);
    public Class GetClassById(int classId);
    public Task<int> AddClassAsync(Class newClass);
    public Task<int> UpdateClassAsync(Class updatedClass);
    public Task<int> DeleteClassAsync(int classId);
}