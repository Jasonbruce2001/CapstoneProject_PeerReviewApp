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

    public AssignmentSubmissionRepository(ApplicationDbContext context, IAssignmentVersionRepository assignmentVersionRepository)
    {
        _context = context;
        _assignmentVersionRepository = assignmentVersionRepository;
    }

    public async Task<IList<AssignmentSubmission>> GetAllSubmissionsByStudentAsync(AppUser user)
    {
        return await _context.AssignmentSubmissions.Where(s => s.Submitter == user).ToListAsync();
    }

    public async Task<IList<AssignmentSubmission>> GetSubmissionsByReviewerAsync(AppUser user)
    {
        return await _context.AssignmentSubmissions
            .Include(s => s.Review)
            .ThenInclude(r => r.Reviewee)
            .Include(s => s.AssignmentVersion)
            .ThenInclude(av => av.ParentAssignment)
            .ThenInclude(a => a.Course)
            .Where(s => s.Review.Reviewer.Id == user.Id)
            .ToListAsync();
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

    public async Task<int> UpdateAssignmentSubmissionAsync(AssignmentSubmission model)
    {
        var oldSubmission = model.AssignmentVersion.Submissions.FirstOrDefault(s => s.Submitter == model.Submitter);

        if (oldSubmission != null)
        {
            //track reference of parent assignment version
            var assignment = await _assignmentVersionRepository.GetAssignmentVersionByIdAsync(model.AssignmentVersion.Id);
            //remove old submission from list
            assignment.Submissions.Remove(oldSubmission);
            //add updated submission
            assignment.Submissions.Add(model);
            //update assignment version
            await _assignmentVersionRepository.UpdateAssignmentVersionAsync(assignment);

            //update submission
            _context.AssignmentSubmissions.Update(model);
        }

        return await _context.SaveChangesAsync();
    }

    public Task<int> DeleteAssignmentSubmissionAsync(AssignmentSubmission model)
    {
        throw new NotImplementedException();
    }
}