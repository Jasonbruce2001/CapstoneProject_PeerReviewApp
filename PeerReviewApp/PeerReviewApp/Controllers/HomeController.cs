using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeerReviewApp.Data;
using PeerReviewApp.Models;

namespace PeerReviewApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly IDocumentRepository _documentRepository;

    public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, IDocumentRepository documentRepository)
    {
        _userManager = userManager;
        _logger = logger;
        _documentRepository = documentRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RelTesting()
    {
        return View(_userManager.Users
                                .ToList());
    }
    
    public async Task<IActionResult> Documents()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var documents = await _documentRepository.GetDocumentsByUserAsync(user);
        
        return View(documents);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}