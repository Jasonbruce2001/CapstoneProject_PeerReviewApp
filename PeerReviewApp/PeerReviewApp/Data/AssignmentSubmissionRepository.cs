using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models;

namespace PeerReviewApp.Data;

public class AssignmentSubmissionRepository : IAssignmentSubmissionRepository
{
    private readonly ApplicationDbContext _context; 
    private readonly IAssignmentVersionRepository _assignmentVersionRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    
    public AssignmentSubmissionRepository(ApplicationDbContext context, IAssignmentVersionRepository assignmentVersionRepository, 
        UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _context = context;
        _assignmentVersionRepository = assignmentVersionRepository;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<IList<AssignmentSubmission>> GetAllSubmissionsByStudentAsync(AppUser user)
    {
        return await _context.AssignmentSubmissions.Where(s => s.Submitter == user).ToListAsync();
    }

    public async Task<int> AddAssignmentSubmissionAsync(AssignmentSubmission model)
    {
        _context.AssignmentSubmissions.Add(model);
        
        //track reference of parent assignment version
        var assignment = await _assignmentVersionRepository.GetAssignmentVersionByIdAsync(model.AssignmentVersion.Id);
        //add submission to list
        assignment.Submissions.Add(model);
        //update assignment version
        await _assignmentVersionRepository.UpdateAssignmentVersionAsync(assignment);
        
        return await _context.SaveChangesAsync();
    }

    public Task<int> UpdateAssignmentSubmissionAsync(AssignmentSubmission model)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAssignmentSubmissionAsync(AssignmentSubmission model)
    {
        throw new NotImplementedException();
    }
}