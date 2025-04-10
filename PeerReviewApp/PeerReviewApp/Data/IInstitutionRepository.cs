using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface IInstitutionRepository
{
    public IList<Institution> GetInstitutions();
    public Task<Institution> GetInstitutionByIdAsync(int id);
    public int UpdateInstitution(Institution institution);  
    public int DeleteInstitution(int id);
    public Task<int> AddInstitutionAsync(Institution institution);
    
}