using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface IClassRepository
{
    public Task<IList<Class>> GetClassesAsync();
    public Task<IList<Class>> GetClassesAsync(string id);
    public Task<IList<Class>> GetArchivedClassesAsync();
    public Task<IList<Class>> GetArchivedClassesAsync(string id);
    public Task<IList<Class>> GetCurrentClassesAsync();
    public Task<IList<Class>> GetCurrentClassesAsync(string id);
    public Task<int> Archive(int id);
    public Class GetClass(int classId);
    public Task<int> AddClassAsync(Class newClass);
    public Task<int> UpdateClassAsync(Class updatedClass);
    public Task<int> DeleteClassAsync(int classId);
}