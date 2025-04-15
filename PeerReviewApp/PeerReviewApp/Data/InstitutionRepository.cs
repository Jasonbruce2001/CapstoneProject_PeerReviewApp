using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public class InstitutionRepository : IInstitutionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser>  _userManager;
    
    public InstitutionRepository(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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
    
    public async Task<int> AddInstructorToInstitutionAsync(string institutionCode, string userId)
    {
        var institution = await _context.Institutions.Where(i => i.Code == institutionCode)
                                                     .Include(i => i.Instructors)
                                                     .FirstOrDefaultAsync();
        
        var instructor = await _userManager.FindByIdAsync(userId);

        if (instructor != null)
        {
            institution.Instructors.Add(instructor);
        }
        
        return await _context.SaveChangesAsync();
    }
    
    public async Task<int> AddInstructorToInstitutionByIdAsync(int institutionId, string userId)
    {
        var institution = await _context.Institutions.FindAsync(institutionId);
                                                        
        
        var instructor = await _userManager.FindByIdAsync(userId);

        if (instructor != null)
        {
            institution.Instructors.Add(instructor);
        }
        
        return await _context.SaveChangesAsync();
    }
}