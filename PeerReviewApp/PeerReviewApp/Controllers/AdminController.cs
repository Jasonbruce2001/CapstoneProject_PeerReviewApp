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

        private readonly IInstitutionRepository _institutionRepository;
        private readonly ICourseRepository _courseRepository;


        private static List<string> PasswordResetRequests = new List<string>();
        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context,
                                IInstitutionRepository institutionRepository, ICourseRepository courseRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _institutionRepository = institutionRepository;

            _courseRepository = courseRepository;
        }


        public async Task<IActionResult> Index()
        {
            var recentRequests = GetRecentPasswordResetRequests();

            var dashboard = new AdminDashboardVM
            {
                TotalInstitutions = (await _institutionRepository.GetInstitutionsAsync()).Count(),
                ActiveInstructors = (await _userManager.GetUsersInRoleAsync("Instructor")).Count(),
                ActiveCourses = (await _courseRepository.GetCoursesAsync()).Count(),
                TotalStudents = _userManager.Users.Count(),
                Institutions = await _institutionRepository.GetInstitutionsAsync(),
                RecentActions = new List<string>(),
                RecentPasswordResetRequests = GetRecentPasswordResetRequests()
            };

            return View(dashboard);
        }


        public static void AddPasswordResetRequest(string request)
        {
            PasswordResetRequests.Insert(0, request); // Add to beginning
            if (PasswordResetRequests.Count > 10) // Keep only last 10
            {
                PasswordResetRequests.RemoveAt(PasswordResetRequests.Count - 1);
            }
        }

        private List<string> GetRecentPasswordResetRequests()
        {
            return PasswordResetRequests.ToList();
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
                var institutions = await _institutionRepository.GetInstitutionsAsync();
                var institution = institutions.FirstOrDefault(i => i.Code == model.InstitutionCode);

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
                    await _institutionRepository.AddInstructorToInstitutionAsync(model.InstitutionCode, user.Id);
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

            var vm = new ViewInstructorsVM { institution = institution, allInstructors = notActive };

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

            var vm = new ViewInstructorsVM { institution = institution, allInstructors = notActive };

            return View("ViewInstructors", vm);
        }

        public async Task<IActionResult> ViewCourses()
        {
            var courses = await _courseRepository.GetCoursesAsync();
            var institutions = await _institutionRepository.GetInstitutionsAsync();
            
            var vm = new ViewCoursesVM(){ Courses = courses, Institutions = institutions };
            return View(vm);
        }

        public async Task<IActionResult> FilterCourses(string courseName, int institutionName) //must parse institutionId as int when querying
        {
            var courses = await _courseRepository.GetCoursesAsync();
            var institutions = await _institutionRepository.GetInstitutionsAsync();
            
            var vm = new ViewCoursesVM(){ Courses = courses, Institutions = institutions };
            
            if (institutionName == -1) //if no institution selected
            {   
                if (string.IsNullOrEmpty(courseName)) //search term was empty
                {
                    return RedirectToAction("ViewCourses");
                }

                vm.Courses = await _courseRepository.SearchByNameAsync(courseName);
                //only search term is entered
                return View("ViewCourses", vm);
            }
            else //if an institution is selected
            {
                vm.Courses = await _courseRepository.SearchByInstitutionAsync(String.Empty, institutionName);
                return View("viewCourses", vm);
            }
        }

        private string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public async Task<IActionResult> ManageStudents(string searchTerm = null)
        {
            var studentRole = await _roleManager.FindByNameAsync("Student");
            var students = new List<AppUser>();

            if (studentRole != null)
            {
                students = (await _userManager.GetUsersInRoleAsync(studentRole.Name)).ToList();

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    students = students.Where(s =>
                        s.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        s.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }
            }

            ViewBag.SearchTerm = searchTerm;
            return View(students);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            var student = await _userManager.FindByIdAsync(id);
            if (student != null)
            {
                var result = await _userManager.DeleteAsync(student);

                if (result.Succeeded)
                {
                    TempData["Message"] = "Student deleted successfully.";
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return RedirectToAction("ManageStudents");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelectedStudents(List<string> selectedIds)
        {
            if (selectedIds != null && selectedIds.Any())
            {
                int successCount = 0;

                foreach (var id in selectedIds)
                {
                    var student = await _userManager.FindByIdAsync(id);
                    if (student != null)
                    {
                        var result = await _userManager.DeleteAsync(student);
                        if (result.Succeeded)
                        {
                            successCount++;
                        }
                    }
                }

                TempData["Message"] = $"{successCount} student(s) deleted successfully.";
            }

            return RedirectToAction("ManageStudents");
        }

        return RedirectToAction("ManageStudents");
    }
}
        public async Task<IActionResult> ResetStudentPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ResetStudentPasswordVM
            {
                StudentId = id,
                StudentName = user.UserName,
                Email = user.Email
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetStudentPassword(ResetStudentPasswordVM model)
        {

            // No need to remove ModelState entries anymore since Token is not required
            if (!ModelState.IsValid)
            {
                // Reload user data when returning view with validation errors
                var userForView = await _userManager.FindByIdAsync(model.StudentId);
                if (userForView != null)
                {
                    model.StudentName = userForView.UserName;
                    model.Email = userForView.Email;
                }
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.StudentId);
            if (user == null)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

            if (result.Succeeded)
            {
                TempData["Message"] = "Student password has been reset successfully.";
                return RedirectToAction("ManageStudents");
            }

            // Reload user data when returning view with password reset errors
            model.StudentName = user.UserName;
            model.Email = user.Email;

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}