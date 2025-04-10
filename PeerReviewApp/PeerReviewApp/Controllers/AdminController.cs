using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Identity;
using PeerReviewApp.Models;
using PeerReviewApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PeerReviewApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        public IActionResult Index()
        {
            var dashboard = new AdminDashboardVM

            {
                TotalInstitutions = _context.Institution.Count(),
                ActiveInstructors = _context.Users.Count(u => u.InstructorCode != null),
                ActiveCourses = _context.Courses.Count(),
                TotalStudents = _context.Users.Count(),
                Institutions = _context.Institution.ToList(),
                RecentActions = new List<string>()
            };
            
            return View(dashboard);
        }


        public async Task<IActionResult> ManageInstructors()
        { 
            var instructors = await _context.Users 
                .Where(u => u.InstructorCode!= null)
                .ToListAsync();

            return View(instructors);
        }

        public IActionResult CreateInstructor()
        { 
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateInstructor(CreateInstructorVM model)
        {
            if (ModelState.IsValid)
            {
                //Generate a random instructorcode 
                string instructorCode = GenerateRandomCode(6);

                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    InstructorCode = instructorCode, 
                    AccountAge = DateTime.Now 
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) 
                {
                    await _userManager.AddToRoleAsync(user, "Instructor");
                    return RedirectToAction("ManageInstructors");   
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
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
