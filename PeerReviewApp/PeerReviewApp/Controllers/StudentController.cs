using Microsoft.AspNetCore.Authorization;
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
    private readonly IAssignmentVersionRepository _assignmentVersionRepository;
    private readonly IDocumentRepository _documentRepository;

    public StudentController(IClassRepository classRepository, IReviewGroupRepository reviewGroupRepository,
        UserManager<AppUser> userManager, IAssignmentVersionRepository assignmentVersionRepository, IDocumentRepository documentRepository)
    {
        _classRepository = classRepository;
        _reviewGroupRepository = reviewGroupRepository;
        _assignmentVersionRepository = assignmentVersionRepository;
        _userManager = userManager;
        _documentRepository = documentRepository;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        
        var classes = await _classRepository.GetClassesForStudentAsync(user);
        var reviewGroups = new List<ReviewGroup>(); //temp change to repo calls
        var documents = new List<Document>();
        
        var model = new StudentDashVM(){ Classes = classes, User = user, ReviewGroups = reviewGroups, Documents = documents };
        
        return View(model);
    }

    public async Task<IActionResult> Assignments()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        
        var assignments = await _assignmentVersionRepository.GetAssignmentVersionsForStudentAsync(user);
        
        return View(assignments);
    }
}