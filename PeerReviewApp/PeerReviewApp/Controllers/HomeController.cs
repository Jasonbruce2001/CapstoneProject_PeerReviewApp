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
    private readonly int MAX_FILE_SIZE = 20971520; //20 mb in bytes
    private readonly IList<string> ALLOWED_EXTENSIONS = new List<string> {".txt", ".docx", ".odt", ".pdf", ".zip", ".md", ".html"};

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

    public IActionResult Upload()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] Document model)
    {
        model.Uploader = await _userManager.GetUserAsync(HttpContext.User);
        model.DateUploaded = DateTime.Now;
        
        if (model.File == null && model.File.Length == 0)
        {
            return BadRequest("Invalid file");
        }

        if (model.File.Length > MAX_FILE_SIZE)
        {
            return BadRequest("File size is too big");
        }
            
        var folderName = Path.Combine("wwwroot", "StaticFiles", "UserUploads");
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        if (!Directory.Exists(pathToSave))
        {
            Directory.CreateDirectory(pathToSave);
        }
        
        //strip extension from name
        var extension = Path.GetExtension(model.File.FileName);

        if (!ALLOWED_EXTENSIONS.Contains(extension))
        {
            return BadRequest("Invalid file extension");
        }
        
        //generate unique GUID for filename 
        var fileName = $"{Guid.NewGuid()}{extension}";
        // c:// res/all/filename
        var fullPath = Path.Combine(pathToSave, fileName);
        var dbPath = Path.Combine(folderName, fileName).Substring(8); //for use in database

        if (System.IO.File.Exists(fullPath))
        {
            return BadRequest("File already exists");
        }

        model.FilePath = fileName;
        model.FileSize = SizeSuffix(model.File.Length);
        
        await _documentRepository.AddDocumentAsync(model);
        
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            model.File.CopyTo(stream);
        }
        
        return RedirectToAction("Documents");
    }

    public async Task<IActionResult> DeleteUpload(int id)
    {
        var doc = await _documentRepository.GetDocumentByIdAsync(id);
        var path = doc.FilePath;
        
        _documentRepository.DeleteDocumentAsync(doc.Id);
        
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
        
        _logger.LogInformation($"Deleted document at {path}", path);

        return RedirectToAction("Documents");
    }
    
    public IActionResult Privacy()
    {
        var doc = new Document();

        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    //size helper
    static readonly string[] SizeSuffixes = 
        { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

    static string SizeSuffix(Int64 value, int decimalPlaces = 1)
    {
        if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); } 

        int i = 0;
        decimal dValue = (decimal)value;
        while (Math.Round(dValue, decimalPlaces) >= 1000)
        {
            dValue /= 1024;
            i++;
        }

        return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
    }
}