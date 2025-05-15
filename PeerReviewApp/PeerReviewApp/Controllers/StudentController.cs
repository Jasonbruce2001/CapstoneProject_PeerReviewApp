using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Data;
using PeerReviewApp.Models;
using PeerReviewApp.Models.ViewModels;

namespace PeerReviewApp.Controllers;


public class StudentController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IClassRepository _classRepository;
    private readonly IReviewGroupRepository _reviewGroupRepository;
    private readonly IAssignmentVersionRepository _assignmentVersionRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IGradeRepository _gradeRepository;
    private readonly ApplicationDbContext _context;
    private readonly IAssignmentSubmissionRepository _assignmentSubmissionRepository;

    public StudentController(IClassRepository classRepository, IReviewGroupRepository reviewGroupRepository, IReviewRepository reviewRepository,
        IGradeRepository gradeRepository, UserManager<AppUser> userManager, ApplicationDbContext context, IAssignmentVersionRepository assignmentVersionRepository, 
        IDocumentRepository documentRepository, IAssignmentSubmissionRepository assignmentSubmissionRepository)
    {
        _classRepository = classRepository;
        _reviewGroupRepository = reviewGroupRepository;
        _assignmentVersionRepository = assignmentVersionRepository;
        _assignmentSubmissionRepository = assignmentSubmissionRepository;
        _userManager = userManager;
        _documentRepository = documentRepository;
        _context = context;
        _reviewRepository = reviewRepository;
        _gradeRepository = gradeRepository;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        
        var classes = await _classRepository.GetClassesForStudentAsync(user);
        var reviewGroups = new List<ReviewGroup>(); //temp change to repo calls
        var documents = new List<Document>();
       
        if (user != null)
        {
           
            try
            {
                classes = (await _classRepository.GetClassesForStudentAsync(user)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        var model = new StudentDashVM(){ Classes = classes, User = user, ReviewGroups = reviewGroups, Documents = documents };
        
        return View(model);
    }

    public async Task<IActionResult> Assignments()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        
        var assignments = await _assignmentVersionRepository.GetAssignmentVersionsForStudentAsync(user);
        
        return View(assignments);
    }
    
    public async Task<IActionResult> DetailedAssignment(int assignmentId)
    {
        var assignment = await _assignmentVersionRepository.GetAssignmentVersionByIdAsync(assignmentId);
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var submissionStatus = false;
        AssignmentSubmission userSub = null;

        //check through submissions for user
        foreach (var submission in assignment.Submissions)
        {
            if (submission.Submitter == user)
            {
                submissionStatus = true;
                userSub = submission;
            }
        }
        
        var vm = new DetailedAssignmentVM(){Assignment = assignment, HasSubmitted = submissionStatus, Submission = userSub};
        return View(vm);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddSubmission(AssignmentSubmission model, int versionId)
    {
        model.AssignmentVersion = await _assignmentVersionRepository.GetAssignmentVersionByIdAsync(versionId);
        model.SubmissionDate = DateTime.Now;
        model.Submitter = await _userManager.GetUserAsync(HttpContext.User);
        
        //add new submission to db
        await _assignmentSubmissionRepository.AddAssignmentSubmissionAsync(model);
        
        return RedirectToAction("Assignments");
    }

    public async Task<IActionResult> ViewCourses()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var classes = await _classRepository.GetClassesForStudentAsync(user);

        var enhancedClasses = new List<Class>();
        foreach (var cls in classes)
        {
            var fullClass = await _classRepository.GetClassByIdAsync(cls.ClassId);
            if (fullClass != null)
            {
                enhancedClasses.Add(fullClass);
            }
        }

        return View(enhancedClasses);



      
    }

    public async Task<IActionResult> ViewGroups()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        var studentAssignmentVersions = await _context.AssignmentVersions
            .Include(av => av.ParentAssignment)
            .ThenInclude(a => a.Course)
            .Include(av => av.Students)
            .Where(av => av.Students.Any(s => s.Id == currentUser.Id))
            .ToListAsync();

        return View(studentAssignmentVersions);
    }

    public async Task<IActionResult> ViewReviews()
    {

        var currentUser = await _userManager.GetUserAsync(User);

        var model = await _reviewRepository.GetReviewsByReviewerAsync(currentUser);

        return View(model);
    }

    public async Task<IActionResult> ViewReceivedReviews()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        var model = await _reviewRepository.GetReviewsByRevieweeAsync(currentUser);

        return View(model);
    }

    public async Task<IActionResult> ViewGrades()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        var model = await _gradeRepository.GetGradesByStudentAsync(currentUser);

        return View(model);
    }

    public async Task<IActionResult> DueSoon()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var temp = await _userManager.Users
            .Include(r => r.Versions)
            .ThenInclude(r => r.ParentAssignment)
            .ThenInclude(r => r.Course)
            .FirstOrDefaultAsync(r => r.Id == currentUser.Id);

        IList<AssignmentVersion> model = temp.Versions;
        foreach (AssignmentVersion version in model.ToList())
        {
            if(!(version.ParentAssignment.DueDate > DateTime.Now && version.ParentAssignment.DueDate < DateTime.Now.AddDays(7)))
            {
                model.Remove(version);
            }
        }

        return View(model);
    }
}