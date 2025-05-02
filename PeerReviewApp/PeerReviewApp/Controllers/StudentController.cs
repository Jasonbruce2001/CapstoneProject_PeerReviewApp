using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeerReviewApp.Data;
using PeerReviewApp.Models;

namespace PeerReviewApp.Controllers;

public class StudentController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IClassRepository _classRepository;
    private readonly IReviewGroupRepository _reviewGroupRepository;

    public StudentController(IClassRepository classRepository, IReviewGroupRepository reviewGroupRepository,
        UserManager<AppUser> userManager)
    {
        _classRepository = classRepository;
        _reviewGroupRepository = reviewGroupRepository;
        _userManager = userManager;
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
            // Get the complete class with ParentCourse loaded
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



        return View(classes);
    }
}