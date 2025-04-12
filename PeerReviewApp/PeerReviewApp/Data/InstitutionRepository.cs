using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public class InstitutionRepository : IInstitutionRepository
{
    private readonly ApplicationDbContext _context;

    public InstitutionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IList<Institution>> GetInstitutionsAsync()
    {
        return await _context.Institutions.ToListAsync();
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

    public async Task<int> UpdateInstitutionAsync(Institution institution)
    {
        _context.Institutions.Update(institution); 
        
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteInstitutionAsync(int id)
    {
        Institution institution = await _context.Institutions.FindAsync(id);

        if (institution != null)
        {
            _context.Institutions.Remove(institution);
        }
        
        return await _context.SaveChangesAsync();
    }
}