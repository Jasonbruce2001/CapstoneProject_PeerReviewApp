using Microsoft.AspNetCore.Authorization;
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
            var user = new AppUser() { UserName = model.Username, AccountAge = DateTime.Now, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (model.InstructorCode == null) //if instructor code is null, assign "Student" role
                {
                    await _userManager.AddToRoleAsync(user, "Student");
                }
                else //if instructor code is provided, assign "Instructor" role
                {
                    if (ValidateInstructorCode(model.InstructorCode))
                    {
                        await _userManager.AddToRoleAsync(user, "Instructor");
                        await _institutionRepository.AddInstructorToInstitutionAsync(model.InstructorCode, user.Id);
                    }
                }

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
                    if (User.IsInRole("Instructor"))
                    {
                        return RedirectToAction("Index", "Instructor");
                    }
                    
                    if (User.IsInRole("Student"))
                    {
                        return RedirectToAction("Index", "Student");
                    }
                    
                    //if no role, should only happen in error
                    return RedirectToAction("Index", "Home");
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

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var logger = HttpContext.RequestServices.GetService<ILogger<AccountController>>();
        logger?.LogInformation($"Password reset requested for {model.Email}");

        AdminController.AddPasswordResetRequest($"{model.Email} requested password reset at {DateTime.Now:MM/dd/yyyy HH:mm}");

        return RedirectToAction("ForgotPasswordConfirmation");
    }

        [HttpGet]
    public IActionResult ResetPassword(string email, string token)
    {
        if (email == null || token == null)
        {
            return BadRequest("Email and token are required");
        }

        var model = new ResetStudentPasswordVM { Email = email, Token = token };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetStudentPasswordVM model)
    {
        // For user self-service resets, Token is required
        if (string.IsNullOrEmpty(model.Token))
        {
            ModelState.AddModelError(nameof(model.Token), "The Token field is required.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return RedirectToAction("ResetPasswordConfirmation");
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("ResetPasswordConfirmation");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }


    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (result.Succeeded)
        {
            // Sign the user back in to refresh the security stamp
            await _signInManager.RefreshSignInAsync(user);
            TempData["Message"] = "Your password has been changed successfully.";

            // Redirect based on user role
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (User.IsInRole("Instructor"))
            {
                return RedirectToAction("Index", "Instructor");
            }
            if (User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Student");
            }

            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }




}

