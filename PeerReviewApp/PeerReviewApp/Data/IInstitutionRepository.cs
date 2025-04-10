using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface IInstitutionRepository
{
    public IList<Institution> GetInstitutions();
    public Institution GetInstitution(int id);
    public Task<int> AddInstitutionAsync(Institution institution);
    public Task<int> UpdateInstitutionAsync(Institution institution);  
    public Task<int> DeleteInstitutionAsync(Institution institution);
}