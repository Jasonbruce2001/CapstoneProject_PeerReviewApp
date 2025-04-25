using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PeerReviewApp.Models;
using PeerReviewApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace PeerReviewApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IInstitutionRepository _institutionRepository;

        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IInstitutionRepository institutionRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _institutionRepository = institutionRepository;
            _context = context;
        }


        public IActionResult Index()
        {
            var dashboard = new AdminDashboardVM
            {
                TotalInstitutions = _context.Institutions.Count(),
                ActiveInstructors = _roleManager.Roles.Where(r => r.Name != "Instructor").Count(),
                ActiveCourses = _context.Courses.Count(),
                TotalStudents = _context.Users.Count(),
                Institutions = _context.Institutions.ToList(),
                RecentActions = new List<string>()
            };

            return View(dashboard);
        }


        public async Task<IActionResult> ManageInstructors()
        {
            var instructorRole = await _roleManager.FindByNameAsync("Instructor");
            var instructors = new List<AppUser>();

            if (instructorRole != null)
            {
                instructors = (await _userManager.GetUsersInRoleAsync(instructorRole.Name)).ToList();
            }

            return View(instructors);
            
        }

        public IActionResult CreateInstructor()
        {
            var model = new CreateInstructorVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInstructor(CreateInstructorVM model)
        {
            if (ModelState.IsValid)
            {
                var institution = await _context.Institutions
                    .FirstOrDefaultAsync(i => i.Code == model.InstitutionCode);

                if (institution == null)
                {
                    ModelState.AddModelError("InstitutionCode", "Invalid institution code");
                    return View(model);
                }


                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    AccountAge = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Instructor");

                    institution.Instructors.Add(user);
                    await _context.SaveChangesAsync();



                    return RedirectToAction("ManageInstructors");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> DeactivateInstructor(string id)
        {
            var instructor = await _userManager.FindByIdAsync(id);
            if (instructor != null)
            {
                var result = await _userManager.DeleteAsync(instructor);

                if (result.Succeeded)
                {
                    TempData["Message"] = "Instructor Deleted Sucussfully.";
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return RedirectToAction("ManageInstructors");
        }


        public async Task<IActionResult> ResetInstructorPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ResetInstructorPasswordVM
            {
                InstructorId = id
            };

            return View(model);

        }



        [HttpPost]
        public async Task<IActionResult> ResetInstructorPassword(ResetInstructorPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.InstructorId);
            if (user == null)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["Message"] = "Password has been reset successfully.";
                return RedirectToAction("ManageInstructors");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);

        }

        public IActionResult ManageInstitutions()
        {
            var institutions = _institutionRepository.GetInstitutionsAsync().Result.ToList();
            return View(institutions);
        }
        
        public IActionResult CreateInstitution()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateInstitution(Institution institution)
        {
            institution.Code = GenerateRandomCode(6);
            
            await _institutionRepository.AddInstitutionAsync(institution);
            
            return RedirectToAction("ManageInstitutions");
        }
        
        
        public IActionResult ViewInstructors(int institutionId)
        {
            var institution = _institutionRepository.GetInstitutionByIdAsync(institutionId).Result;
            var users = _userManager.GetUsersInRoleAsync("Instructor").Result.ToList();

            var notActive = new List<AppUser>();

            foreach (AppUser u in users)
            {
                if (!institution.Instructors.Contains(u))
                {
                    notActive.Add(u);
                }
            }
            
            var vm = new ViewInstructorsVM { institution = institution,  allInstructors = notActive };
            
            return View(vm);
        }

        public async Task<IActionResult> AssignInstructor(int institutionId, string instructorId)
        {
            await _institutionRepository.AddInstructorToInstitutionByIdAsync(institutionId, instructorId);
            
            var institution = _institutionRepository.GetInstitutionByIdAsync(institutionId).Result;
            var users = _userManager.GetUsersInRoleAsync("Instructor").Result.ToList();

            var notActive = new List<AppUser>();

            foreach (AppUser u in users)
            {
                if (!institution.Instructors.Contains(u))
                {
                    notActive.Add(u);
                }
            }
            
            var vm = new ViewInstructorsVM { institution = institution,  allInstructors = notActive };
            
            return View("ViewInstructors", vm);
        }
        
        private string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}