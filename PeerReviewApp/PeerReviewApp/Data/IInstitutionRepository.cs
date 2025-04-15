using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public interface IInstitutionRepository
{
    public Task<IList<Institution>> GetInstitutionsAsync();
    public Task<Institution> GetInstitutionByIdAsync(int id);
    public Task<int> AddInstitutionAsync(Institution institution);
    public Task<int> UpdateInstitutionAsync(Institution institution);  
    public Task<int> DeleteInstitutionAsync(int id);
    public Task<int> AddInstructorToInstitutionAsync(string institutionId, string userId); //by join code
    public Task<int> AddInstructorToInstitutionByIdAsync(int institutionId, string userId);
}