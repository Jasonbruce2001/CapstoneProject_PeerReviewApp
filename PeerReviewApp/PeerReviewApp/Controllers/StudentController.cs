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
    private readonly IAssignmentSubmissionRepository _assignmentSubmissionRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IGradeRepository _gradeRepository;


  

    public StudentController(IClassRepository classRepository, IReviewGroupRepository reviewGroupRepository, IReviewRepository reviewRepository,
        IGradeRepository gradeRepository, UserManager<AppUser> userManager, IAssignmentVersionRepository assignmentVersionRepository,

        IDocumentRepository documentRepository, IAssignmentSubmissionRepository assignmentSubmissionRepository)
    {
        _classRepository = classRepository;
        _reviewGroupRepository = reviewGroupRepository;
        _assignmentVersionRepository = assignmentVersionRepository;
        _assignmentSubmissionRepository = assignmentSubmissionRepository;
        _userManager = userManager;
        _documentRepository = documentRepository;
        _assignmentSubmissionRepository = assignmentSubmissionRepository;
        _reviewRepository = reviewRepository;
        _gradeRepository = gradeRepository;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        
        var classes = await _classRepository.GetClassesForStudentAsync(user);
        var reviewsToDo = await _assignmentSubmissionRepository.GetSubmissionsByReviewerAsync(user);
        var studentSubmissions = await _assignmentSubmissionRepository.GetAllSubmissionsByStudentAsync(user);
        
        var reviewsReceived = new List<Review>();

        foreach (var a in studentSubmissions)
        {
            //if a students submission has a review document added to it, add the review to reviewsReceived list
            if (a.Review is { ReviewDocument: not null })
            {
                reviewsReceived.Add(a.Review);
            }
        }
        
        //temp change to repo calls
        var reviewGroups = new List<ReviewGroup>(); 
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

        var model = new StudentDashVM(){ Classes = classes, User = user, ReviewGroups = reviewGroups, Documents = documents, 
                                        ReviewsToDo = reviewsToDo, ReviewsReceived = reviewsReceived};
        
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
        
        //attempt automatic partner assignment
        var result = await _assignmentSubmissionRepository.CheckForPartner(model);
        
        return RedirectToAction("Assignments");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSubmission(AssignmentSubmission model, int versionId)
    {
        model.AssignmentVersion = await _assignmentVersionRepository.GetAssignmentVersionByIdAsync(versionId);
        model.SubmissionDate = DateTime.Now;
        model.Submitter = await _userManager.GetUserAsync(HttpContext.User);
        
        //update submission
        await _assignmentSubmissionRepository.UpdateAssignmentSubmissionAsync(model);
        
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

        var studentAssignmentVersions = await _assignmentVersionRepository.GetAssignmentVersionsForStudentAsync(currentUser);
        return View(studentAssignmentVersions);
    }

    public async Task<IActionResult> ViewReviews()
    {

        var currentUser = await _userManager.GetUserAsync(User);

        var model = await _assignmentSubmissionRepository.GetSubmissionsByReviewerAsync(currentUser);

        return View(model);
    }

    public async Task<IActionResult> ViewReceivedReviews()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        //all assigned submissions, not ones that have had reviews submitted to
        var reviews = await _reviewRepository.GetReviewsByRevieweeAsync(currentUser);

        var model = new List<Review>();

        foreach (var r in reviews)
        {
            if (r.ReviewDocument != null)
            {
                model.Add(r); //only reviews that have had a review document submitted
            }
        }

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
        var versions = await _assignmentVersionRepository.GetAssignmentVersionsForStudentAsync(currentUser);

        var dueVersions = versions
            .Where(v => v.ParentAssignment.DueDate > DateTime.Now
                     && v.ParentAssignment.DueDate < DateTime.Now.AddDays(7))
            .ToList();

        return View(dueVersions);
    }

    public async Task<IActionResult> SubmitReview(int reviewId)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var documents = await _documentRepository.GetDocumentsByUserAsync(user);

        SubmitReviewVM model = new SubmitReviewVM() { Documents = documents, ReviewId = reviewId };

        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> SubmitReview(SubmitReviewVM model)
    {
        Document doc = await _documentRepository.GetDocumentByIdAsync(model.DocumentId);
        Review review = await _reviewRepository.GetReviewByIdAsync(model.ReviewId);
        review.ReviewDocument = doc;

        await _reviewRepository.UpdateReviewAsync(review);

        return RedirectToAction("ViewReviews");
    }
}