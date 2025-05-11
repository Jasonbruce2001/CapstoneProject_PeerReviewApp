using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PeerReviewApp.Data;
using PeerReviewApp.Models;
using Microsoft.EntityFrameworkCore;

namespace PeerReviewApp.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorController : Controller
    {
        //Need to add role restriction to instructors here
        private readonly ILogger<InstructorController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICourseRepository _courseRepo;
        private readonly IInstitutionRepository _institutionRepo;
        private readonly IClassRepository _classRepo;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly IAssignmentVersionRepository _assignmentVersionRepo;
        private readonly IDocumentRepository _documentRepo;
        private readonly ApplicationDbContext _context;

        public InstructorController(ILogger<InstructorController> logger, UserManager<AppUser> userManager, ICourseRepository courseRepo, IInstitutionRepository instRepo, IClassRepository classRepo, SignInManager<AppUser> signInMngr, IAssignmentVersionRepository assignmentVersionRepo, IAssignmentRepository assignmentRepository, IDocumentRepository documentRepository, ApplicationDbContext context)


        {
            _userManager = userManager;
            _signInManager = signInMngr;
            _logger = logger;
            _courseRepo = courseRepo;
            _institutionRepo = instRepo;
            _classRepo = classRepo;
            _assignmentVersionRepo = assignmentVersionRepo;
            _assignmentRepo = assignmentRepository;
            _documentRepo = documentRepository;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            
            // send user to login if not logged in
            if (!_signInManager.IsSignedIn(User))
            {
                var returnURL = Request.GetEncodedUrl();
                return RedirectToAction("Login", "Account", returnURL);
            }
            
            var courses = await _classRepo.GetCoursesForInstructorAsync(user);
            var classes = await _classRepo.GetClassesForInstructorAsync(user);
            var students = new List<AppUser>();

            foreach (Class c in classes) //add all students from instructor's classes to one list
            {
                students.AddRange(c.Students);
            }
            
            var viewModel = new InstructorDashVM { Instructor = user,  Classes = classes, Courses = courses, Students = students };
            
            return View("Index", viewModel);
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

        public async Task<IActionResult> ViewStudents()
        {
            var user = _userManager.GetUserAsync(User).Result;

            // send user to login if not logged in
            if (!_signInManager.IsSignedIn(User))
            {
                var returnURL = Request.GetEncodedUrl();
                return RedirectToAction("Login", "Account", returnURL);
            }

            //get classes for current instructor
            var classes = await _classRepo.GetClassesForInstructorAsync(user);
            
            return View("ViewStudents", classes);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(string studentId, int classId)
        {
            if (await _classRepo.DeleteStudentFromClassAsync(classId, studentId) > 0)
            {
                return RedirectToAction("ViewStudents");
            }
            else
            {
                ViewBag.ErrorMessage = "There was an error adding the course.";
                return RedirectToAction("ViewStudents");
            }


        }

        public IActionResult AddCourse()
        {
            //Get list of institutions to display for course
            IList<Institution> inst = _institutionRepo.GetInstitutionsAsync().Result;
            AddCourseVM vm = new AddCourseVM { Institutions = inst };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseVM model)
        {
            //get institution linked to course
            Institution inst = await _institutionRepo.GetInstitutionByIdAsync(model.InstId);
            model.Course.Institution = inst;

            if (model.Course.Institution != null)
            {
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
            else { return RedirectToAction("AddCourse"); }
        }

        [HttpGet]
        public async Task<IActionResult> EditCourse(int Id)
        {
            //Get list of institutions to display for course
            IList<Institution> inst = _institutionRepo.GetInstitutionsAsync().Result;
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

        public async Task<IActionResult> AddAssignment(int classId)
        {
            var user = await _userManager.GetUserAsync(User);
            var class_ = await _context.Classes
                .Include(c => c.ParentCourse)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (class_ == null || class_.Instructor.Id != user.Id)
            {
                return NotFound();
            }

            var model = new AddAssignmentVM
            {
                ClassId = classId,
                ClassName = class_.ParentCourse.Name,
                Term = class_.Term,
                DueDate = DateTime.Now.AddDays(7)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAssignment(AddAssignmentVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            var class_ = await _context.Classes
                .Include(c => c.ParentCourse)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.ClassId == model.ClassId);

            if (class_ == null || class_.Instructor.Id != user.Id)
            {
                return NotFound();
            }

            var assignment = new Assignment
            {
                Title = model.Title,
                DueDate = model.DueDate,
                Course = class_.ParentCourse
            };

            await _assignmentRepo.AddAssignmentAsync(assignment);

            TempData["Message"] = "Assignment added successfully.";
            return RedirectToAction("ViewAssignments", new { classId = model.ClassId });
        }



        public async Task<IActionResult> ViewAssignments(int classId)
        {

            var user = await _userManager.GetUserAsync(User);
            var class_ = await _context.Classes 
                .Include(c => c.ParentCourse)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (class_ == null || class_.Instructor.Id != user.Id)
            {
                return NotFound();
            }


            var assignments = await _assignmentRepo.GetAssignmentsByCourseAsync(class_.ParentCourse.Id);
            ViewBag.ClassId = classId;
            ViewBag.ClassName = class_.ParentCourse.Name;
            ViewBag.Term = class_.Term;

            return View(assignments);
        }

        public async Task<IActionResult> EditAssignment(int assignmentId)
        {
            var user = await _userManager.GetUserAsync(User);
            var assignment = await _assignmentRepo.GetAssignmentByIdAsync(assignmentId);

            if (assignment == null)
            {
                return NotFound();
            }

           
            var classes = await _classRepo.GetClassesAsync(user.Id);
            bool hasAccess = classes.Any(c => c.ParentCourse.Id == assignment.Course.Id);

            if (!hasAccess)
            {
                return Forbid();
            }

            var model = new EditAssignmentVM
            {
                Id = assignment.Id,
                Title = assignment.Title,
                DueDate = assignment.DueDate,
                ClassId = classes.First(c => c.ParentCourse.Id == assignment.Course.Id).ClassId
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> EditAssignment(EditAssignmentVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var assignment = await _assignmentRepo.GetAssignmentByIdAsync(model.Id);

            if (assignment == null)
            {
                return NotFound();
            }

            // Update assignment properties
            assignment.Title = model.Title;
            assignment.DueDate = model.DueDate;

            await _assignmentRepo.UpdateAssignmentAsync(assignment);

           
            return RedirectToAction("ViewAssignments", new { classId = model.ClassId });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAssignment(int assignmentId, int classId)
        {
            var assignment = await _assignmentRepo.GetAssignmentByIdAsync(assignmentId);

            if (assignment == null)
            {
                return NotFound();
            }

            await _assignmentRepo.DeleteAssignmentAsync(assignmentId);

            
            return RedirectToAction("ViewAssignments", new { classId });
        }


        public async Task<IActionResult> ViewSubmissions(int assignmentId)
        {
            var user = await _userManager.GetUserAsync(User);
            var assignment = await _assignmentRepo.GetAssignmentByIdAsync(assignmentId);

            if (assignment == null)
                return NotFound();

            // Find a class that this instructor teaches with this assignment's course
            var classes = await _classRepo.GetClassesAsync(user.Id);
            var classForCourse = classes.FirstOrDefault(c => c.ParentCourse.Id == assignment.Course.Id);

            if (classForCourse == null)
                return Forbid();

            // Retrieve reviews that have this assignment
            var reviews = await _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.ReviewDocument)
                .ToListAsync();

            // Extract documents from reviews
            var submissions = reviews
                .Where(r => r.ReviewDocument != null)
                .Select(r => r.ReviewDocument)
                .ToList();

            ViewBag.AssignmentTitle = assignment.Title;
            ViewBag.DueDate = assignment.DueDate;
            ViewBag.ClassId = classForCourse.ClassId;

            return View(submissions);
        }


        public async Task<IActionResult> AddStudents(int classId)
        {
            var user = await _userManager.GetUserAsync(User);

            // Get the class with its related entities
            var class_ = await _context.Classes
                .Include(c => c.ParentCourse)
                .Include(c => c.Instructor)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (class_ == null || class_.Instructor.Id != user.Id)
            {
                return NotFound();
            }

            // Get IDs of students already in the class
            var existingStudentIds = class_.Students.Select(s => s.Id).ToList();

            // Get all users who aren't already in the class
            var availableStudents = await _userManager.GetUsersInRoleAsync("Student");
            availableStudents = availableStudents.Where(u => !existingStudentIds.Contains(u.Id)).ToList();

            var model = new AddStudentsVM
            {
                ClassId = classId,
                ClassName = class_.ParentCourse.Name,
                Term = class_.Term,
                AvailableStudents = availableStudents.ToList(),
                SelectedStudentIds = new List<string>()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudents(AddStudentsVM model)
        {
            if (model.SelectedStudentIds == null || !model.SelectedStudentIds.Any())
            {
                TempData["ErrorMessage"] = "Please select at least one student to add.";
                return RedirectToAction(nameof(AddStudents), new { classId = model.ClassId });
            }

            // Get the class with its students
            var class_ = await _context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.ClassId == model.ClassId);

            if (class_ == null)
            {
                return NotFound();
            }

            // Add selected students to class
            foreach (var studentId in model.SelectedStudentIds)
            {
                var student = await _userManager.FindByIdAsync(studentId);
                if (student != null && !class_.Students.Any(s => s.Id == studentId))
                {
                    class_.Students.Add(student);
                }
            }

            await _context.SaveChangesAsync();

            
            return RedirectToAction("ViewStudents", new { instructor = await _userManager.GetUserAsync(User) });
        }


        public async Task<IActionResult> AddAssignmentVersion(int id)
        {
            // send user to login if not logged in
            if (!_signInManager.IsSignedIn(User))
            {
                var returnURL = Request.GetEncodedUrl();
                return RedirectToAction("Login", "Account", returnURL);
            }
            // get appuser for current user
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager != null)
            {
                user = await _userManager.GetUserAsync(User);
            }

            //Get list of Assignments and Documents to display for class
            IList<Document> documents = new List<Document>();
            IList<Assignment> assignments = new List<Assignment>();
            documents = await _documentRepo.GetDocumentsAsync(user.Id);
            assignments = await _assignmentRepo.GetAssignmentsByCourseAsync(id);

            if (documents != null)
            {
                AssignmentVersionVM vm = new AssignmentVersionVM { Assignments = assignments, Documents = documents };

                return View(vm);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAssignmentVersion(AssignmentVersionVM model)
        {
            
            //get Assignment linked to version
            Assignment assignment = await _assignmentRepo.GetAssignmentByIdAsync(model.AssignmentId);
            Document instruction = await _documentRepo.GetDocumentByIdAsync(model.InstructionsId);
            Document reviewForm = await _documentRepo.GetDocumentByIdAsync(model.ReviewFormId);
            model.AssnVersion.ParentAssignment = assignment;
            model.AssnVersion.ReviewForm = reviewForm;
            model.AssnVersion.Instructions = instruction;

            // send user to login if not logged in
            if (!_signInManager.IsSignedIn(User))
            {
                var returnURL = Request.GetEncodedUrl();
                return RedirectToAction("Login", "Account", returnURL);
            }


            //Add AssignmentVersion to database
            if (await _assignmentVersionRepo.AddAssignmentVersionAsync(model.AssnVersion) > 0)
            {
                return RedirectToAction("ViewClasses");
            }
            else
            {
                ViewBag.ErrorMessage = "There was an error adding the course.";
                return RedirectToAction("AddAssignmentVersion");
            }
        }

        public async Task<IActionResult> ViewGroups(int classId)
        {
            Class cls = await _classRepo.GetClassByIdAsync(classId);
            IList<AppUser> students = cls.Students;
            IList<Assignment> assignments = cls.ParentCourse.Assignments;
            ViewGroupsVM vm = new ViewGroupsVM() { Assignments = assignments, Students = students, Class = cls };

            return View(vm);

        }

        public async Task<IActionResult> SortGroup(int classId, int assignmentId)
        {
            Class cls = await _classRepo.GetClassByIdAsync(classId);
            IList<AppUser> students = cls.Students;
            await _assignmentVersionRepo.DeleteStudentsFromAssignmentVersionAsync(students, assignmentId);
            await _assignmentVersionRepo.AddStudentsToAssignmentVersionsAsync(students, assignmentId);

            return RedirectToAction("ViewClasses");

        }
    }
}

