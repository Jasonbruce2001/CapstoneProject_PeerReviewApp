
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
    private readonly IReviewRepository _reviewRepository;

    public AssignmentSubmissionRepository(ApplicationDbContext context, IAssignmentVersionRepository assignmentVersionRepository, IReviewRepository reviewRepository)
    {
        _context = context;
        _assignmentVersionRepository = assignmentVersionRepository;
        _reviewRepository = reviewRepository;
    }

    public async Task<IList<AssignmentSubmission>> GetAllSubmissionsByStudentAsync(AppUser user)
    {
        return await _context.AssignmentSubmissions.Where(s => s.Submitter == user).ToListAsync();
    }

    public async Task<AssignmentSubmission> GetSubmissionByIdAsync(int id)
    {
        return await _context.AssignmentSubmissions
            .Where(s => s.Id == id)
            .Include(s => s.Submitter)
            .Include(s => s.AssignmentGrade)
            .Include(s => s.AssignmentVersion)
            .ThenInclude(av => av.ParentAssignment)
            .Include(s => s.Review)
            .ThenInclude(r => r.ReviewGrade)
            .FirstOrDefaultAsync();
    }

    public async Task<IList<AssignmentSubmission>> GetSubmissionsByAssignmentAsync(int assignmentId)
    {
        //returns all submissions for every assignment version of a parent assignment
        return await _context.AssignmentSubmissions.Where(s => s.AssignmentVersion.ParentAssignment.Id == assignmentId)
            .Include(s => s.Submitter)
            .Include(s => s.AssignmentVersion)
            .ThenInclude(av => av.ParentAssignment)
            .Include(s => s.Review)
            .ThenInclude(r => r.ReviewGrade)
            .Include(s => s.AssignmentGrade)
            .ToListAsync();
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

        //normalize link to assignment for easy href url
        model.AssignmentLink = NormalizeUrl(model.AssignmentLink);
        
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

    public async Task<int> CheckForPartner(AssignmentSubmission model)
    {
        //find submissions where the parent assignment is the same, but not the assignment version
        var submissions = await _context.AssignmentSubmissions
            .Where(s => s.AssignmentVersion.ParentAssignment == model.AssignmentVersion.ParentAssignment 
                                                                    && s.AssignmentVersion.Id != model.AssignmentVersion.Id)
            .Include(a => a.Review)
            .Include(a => a.Submitter)
            .Include(a => a.AssignmentVersion)
            .ThenInclude(av => av.ParentAssignment)
            .ToListAsync();

        //if submissions are found, loop through looking for null review
        if (submissions.Any())
        {
            foreach (var s in submissions)
            {
                //if a submission from a different assignment version has a null review entry, then partners can be assigned
                if (s.Review == null)
                {
                    //first create review objects for assignment submissions
                    var review1 = new Review() { Reviewer = s.Submitter, Reviewee = model.Submitter };
                    var review2 = new Review() { Reviewer = model.Submitter, Reviewee = s.Submitter };
                    
                    //add reviews to db
                    await _reviewRepository.AddReviewAsync(review1);
                    await _reviewRepository.AddReviewAsync(review2);
                    
                    //assign reviews to submissions
                    model.Review = review1;
                    s.Review = review2;

                    //update database entries
                    await UpdateAssignmentSubmissionAsync(model);
                    await UpdateAssignmentSubmissionAsync(s);

                    return 1; //return 1 if partners have been assigned
                }
            }
        }

        return 0; //return 0 if no partner found
    }

    
    //helper methods
    
    //method to ensure https:// prefixes every assignment submission
    private string NormalizeUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return url;

        // Add https:// if missing
        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            url = "https://" + url;
        }

        // Optional: remove duplicate "www." if already covered
        url = url.Replace("http://www.", "https://")
            .Replace("https://www.", "https://");

        return url;
    }

}