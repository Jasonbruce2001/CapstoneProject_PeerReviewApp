using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public class InstitutionRepository : IInstitutionRepository
{
    private readonly ApplicationDbContext _context;

    public InstitutionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IList<Institution> GetInstitutions()
    {
        return _context.Institutions.ToList();
    }

    public async Task<Institution> GetInstitutionByIdAsync(int id)
    {
        var institution = await _context.Institutions.FindAsync(id);
        
        return institution;
    }

    public async Task<int> AddInstitutionAsync(Institution institution)
    {
        await _context.Institutions.AddAsync(institution);
        return await _context.SaveChangesAsync();
    }

    public int UpdateInstitution(Institution institution)
    {
        _context.Institutions.Update(institution); 
        
        return _context.SaveChanges();
    }

    public int DeleteInstitution(int id)
    {
        Institution institution = GetInstitutionByIdAsync(id).Result;
        
        _context.Institutions.Remove(institution);
        return _context.SaveChanges();
    }
}