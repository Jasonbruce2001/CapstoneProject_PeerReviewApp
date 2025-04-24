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
        var classes = new List<Class>();
        var reviewGroups = new List<ReviewGroup>(); 
        var documents = new List<Document>();
        
        var model = new StudentDashVM(){ Classes = classes, User = user, ReviewGroups = reviewGroups, Documents = documents };
        
        return View(model);
    }
}