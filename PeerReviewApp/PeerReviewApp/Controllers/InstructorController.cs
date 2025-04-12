using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PeerReviewApp.Data;
using PeerReviewApp.Models;

namespace PeerReviewApp.Controllers
{
    [Authorize]
    public class InstructorController : Controller
    {
        //Need to add role restriction to instructors here
        private readonly ILogger<InstructorController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private ICourseRepository _courseRepo;
        private IInstitutionRepository _institutionRepo;
        private IClassRepository _classRepo;

        public InstructorController(ILogger<InstructorController> logger, UserManager<AppUser> userManager, ICourseRepository courseRepo, IInstitutionRepository instRepo, IClassRepository classRepo, SignInManager<AppUser> signInMngr)
        {
            _userManager = userManager;
            _signInManager = signInMngr;
            _logger = logger;
            _courseRepo = courseRepo;
            _institutionRepo = instRepo;
            _classRepo = classRepo;
        }
        public IActionResult Index()
        {
            // send user to login if not logged in
            if (!_signInManager.IsSignedIn(User))
            {
                var returnURL = Request.GetEncodedUrl();
                return RedirectToAction("Login", "Account", returnURL);
            }

            return View();
        }

        public async Task<IActionResult> ViewClasses()
        {
            // get appuser for current user
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager != null)
            {
                user = await _userManager.GetUserAsync(User);
            }

            //get classes for current instructor
            var classes = await _classRepo.GetClassesAsync(user.Id);

            return View(classes);
        }

        [HttpPost]
        public async Task<IActionResult> ArchiveClass(int classId)
        {
            int result = await _classRepo.Archive(classId);
            return RedirectToAction("ViewClasses");
        }

        public IActionResult ViewStudents()
        {
            return View();
        }

        public IActionResult AddCourse()
        {
            //Get list of institutions to display for course
            IList<Institution> inst = _institutionRepo.GetInstitutions();
            AddCourseVM vm = new AddCourseVM { Institutions=inst};

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseVM model)
        {
            //get institution linked to course
            Institution inst = await _institutionRepo.GetInstitutionByIdAsync(model.InstId);
            model.Course.Institution = inst;

            if (ModelState.IsValid) { 
                if (await _courseRepo.AddCourseAsync(model.Course) > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "There was an error adding the course.";
                    return View();
                }
            }
            else 
            { 
                return RedirectToAction("AddCourse"); 
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCourse(int Id)
        {
            //Get list of institutions to display for course
            IList<Institution> inst = _institutionRepo.GetInstitutions();
            Course course = await _courseRepo.GetCourseByIdAsync(Id);
            AddCourseVM vm = new AddCourseVM { Institutions = inst, Course = course };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(AddCourseVM model)
        {
            //get institution linked to course
            Institution inst = await _institutionRepo.GetInstitutionByIdAsync(model.InstId);
            model.Course.Institution = inst;


            if (await _courseRepo.UpdateCourseAsync(model.Course) > 0)
            {
                return RedirectToAction("ViewClasses");
            }
            else
            {
                ViewBag.ErrorMessage = "There was an error adding the course.";
                return View();
            }
        }

        public async Task<IActionResult> AddClass()
        {
            //Get list of Courses to display for class
            IList<Course> crs = await _courseRepo.GetCoursesAsync();
            AddClassVM vm = new AddClassVM { Courses = crs };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddClass(AddClassVM model)
        {
            //get course linked to class
            Course crs = await _courseRepo.GetCourseByIdAsync(model.CourseId);
            model.Class.ParentCourse = crs;

            // send user to login if not logged in
            if (!_signInManager.IsSignedIn(User))
            {
                var returnURL = Request.GetEncodedUrl();
                return RedirectToAction("Login", "Account", returnURL);
            }

            // get appuser for current user
            model.Class.Instructor = _userManager.GetUserAsync(User).Result;
            if (_userManager != null)
            {
                model.Class.Instructor = await _userManager.GetUserAsync(User);
            }

            //Add class to database
            if (await _classRepo.AddClassAsync(model.Class) > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "There was an error adding the course.";
                return View();
            }
        }
    }
}
