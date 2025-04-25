using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeerReviewApp.Data;
using PeerReviewApp.Models;

namespace PeerReviewApp.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager; 
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IInstitutionRepository _institutionRepository;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IInstitutionRepository institutionRepository, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager; 
        _signInManager = signInManager;
        _roleManager = roleManager;
        _institutionRepository = institutionRepository;
    } 
    
    // GET
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        if (ModelState.IsValid)
        {
            if (model.InstructorCode == null) //if instructor code is null, signing up as student
            {
                var user = new AppUser() { UserName = model.Username, AccountAge = DateTime.Now, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else //signing up as instructor
            {
                if (ValidateInstructorCode(model.InstructorCode))
                {
                    var roleName = "Instructor";
                    var username = model.Username;
                    var user = new AppUser();
                    var result = IdentityResult.Failed();
                    
                    // if role doesn't exist, create it
                    if (await _roleManager.FindByNameAsync(roleName) == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                    // if username doesn't exist, create it and add it to role
                    if (await _userManager.FindByNameAsync(username) == null)
                    {
                        user = new AppUser { UserName = username, AccountAge = DateTime.Now, Email = model.Email, RoleNames = new List<string> { roleName } };
                        result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, roleName); //add user to isntructor role
                            await _institutionRepository.AddInstructorToInstitutionAsync(model.InstructorCode, user.Id); //add user to added institution
                        }
                    }

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (User.IsInRole("Instructor"))
                        {
                            return RedirectToAction("Index", "Instructor");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Student");
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
        }
        
        return View(model);
    }
    
    [HttpGet]
    public IActionResult LogIn(string returnURL = "")
    {
        var model = new LogInVM { ReturnUrl = returnURL };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LogInVM model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync
                (model.Username, model.Password, model.RememberMe, false);
          
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) &&
                    Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (User.IsInRole("Instructor"))
                    {
                        return RedirectToAction("Index", "Instructor");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Student");
                    }
                }
            }
        }

        ModelState.AddModelError(string.Empty, "Invalid username or password.");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    public bool ValidateInstructorCode(string code)
    {
        var result = false;
        var codes = new List<string>();

        foreach (var i in _institutionRepository.GetInstitutionsAsync().Result)
        {
            codes.Add(i.Code);
        }
        
        if (codes.Contains(code)) result = true;
        
        return result;
    }
}