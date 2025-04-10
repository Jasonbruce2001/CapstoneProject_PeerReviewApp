using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public class ClassRepository : IClassRepository
{
    public IList<Class> GetClasses()
    {
        throw new NotImplementedException();
    }

    public Class GetClass(int classId)
    {
        throw new NotImplementedException();
    }

    public Task<int> AddClassAsync(Class newClass)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateClassAsync(Class updatedClass)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteClassAsync(int classId)
    {
        throw new NotImplementedException();
    }
}