using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public class InstitutionRepository : IInstitutionRepository
{
    public IList<Institution> GetInstitutions()
    {
        throw new NotImplementedException();
    }

    public Institution GetInstitution(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> AddInstitutionAsync(Institution institution)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateInstitutionAsync(Institution institution)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteInstitutionAsync(Institution institution)
    {
        throw new NotImplementedException();
    }
}