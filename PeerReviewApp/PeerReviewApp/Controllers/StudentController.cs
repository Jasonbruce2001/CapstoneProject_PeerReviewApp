using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Data;
using PeerReviewApp.Models;

namespace PeerReviewApp.Controllers;

public class StudentController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IClassRepository _classRepository;
    private readonly IReviewGroupRepository _reviewGroupRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly ApplicationDbContext _context;

    public StudentController(IClassRepository classRepository, IReviewGroupRepository reviewGroupRepository, IReviewRepository reviewRepository,
        UserManager<AppUser> userManager, ApplicationDbContext context)
    {
        _classRepository = classRepository;
        _reviewGroupRepository = reviewGroupRepository;
        _userManager = userManager;
        _context = context;
        _reviewRepository = reviewRepository;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        //temp change to repo calls
        // var classes = new List<Class>();
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
                
            }
        }

        var model = new StudentDashVM(){ Classes = classes, User = user, ReviewGroups = reviewGroups, Documents = documents };
        
        return View(model);
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
}