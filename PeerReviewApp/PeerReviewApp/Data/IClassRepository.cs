using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface IClassRepository
{
    public Task<IList<Class>> GetClasses();
    public Task<IList<Class>> GetArchivedClasses();
    public Task<IList<Class>> GetCurrentClasses();
    public Class GetClass(int classId);
    public Task<int> AddClassAsync(Class newClass);
    public Task<int> UpdateClassAsync(Class updatedClass);
    public Task<int> DeleteClassAsync(int classId);
}