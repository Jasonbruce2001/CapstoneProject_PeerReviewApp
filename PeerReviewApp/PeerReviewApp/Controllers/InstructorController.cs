using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PeerReviewApp.Data;
using PeerReviewApp.Models;
using Microsoft.EntityFrameworkCore;
using PeerReviewApp.Models.ViewModels;

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
        private readonly IAssignmentSubmissionRepository _assignmentSubmissionRepo;
        private readonly IGradeRepository _gradeRepo;
        private readonly IDocumentRepository _documentRepo;
        private readonly IReviewRepository _reviewRepository;
        private readonly IAssignmentSubmissionRepository _submissionRepository;
        private readonly ApplicationDbContext _context;

        public InstructorController(ILogger<InstructorController> logger, UserManager<AppUser> userManager,
            ICourseRepository courseRepo, IInstitutionRepository instRepo, IClassRepository classRepo,
            SignInManager<AppUser> signInMngr, IAssignmentVersionRepository assignmentVersionRepo,
            IAssignmentRepository assignmentRepository, IDocumentRepository documentRepository,
            IReviewRepository reviewRepository, IAssignmentSubmissionRepository submissionRepository,
            IGradeRepository gradeRepository)


        {
            _userManager = userManager;
            _signInManager = signInMngr;
            _logger = logger;
            _courseRepo = courseRepo;
            _institutionRepo = instRepo;
            _classRepo = classRepo;
            _assignmentVersionRepo = assignmentVersionRepo;
            _assignmentRepo = assignmentRepository;
            _assignmentSubmissionRepo = submissionRepository;
            _gradeRepo = gradeRepository;
            _documentRepo = documentRepository;
            _reviewRepository = reviewRepository;
            _submissionRepository = submissionRepository;
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


            var readyForGradingCount = await GetReadyForGradingCountAsync();

            var viewModel = new InstructorDashVM
            {
                Instructor = user, Classes = classes, Courses = courses, Students = students,
                ReadyForGradingCount = readyForGradingCount
            };

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

        public async Task<IActionResult> ViewCourses()
        {

            // get appuser for current user
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager != null)
            {
                user = await _userManager.GetUserAsync(User);
            }

            //get classes for current instructor
            var classes = await _courseRepo.GetCoursesAsync(user);

            return View(classes);
        }

        [HttpPost]
        public async Task<IActionResult> ArchiveClass(int classId)
        {
            int result = await _classRepo.Archive(classId);
            return RedirectToAction("ViewClasses");
        }


        public async Task<IActionResult> ViewStudents(int page = 1, string searchTerm = "")
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

            // Flatten students with their class info
            var allStudents = new List<StudentClassInfo>();
            foreach (var cls in classes)
            {
                foreach (var student in cls.Students)
                {
                    allStudents.Add(new StudentClassInfo
                    {
                        Student = student,
                        Class = cls
                    });
                }
            }


            if (!string.IsNullOrEmpty(searchTerm))
            {
                allStudents = allStudents.Where(s =>
                    s.Student.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    s.Class.ParentCourse.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    s.Class.Term.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // Pagination
            const int pageSize = 10;
            var totalStudents = allStudents.Count;
            var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);


            page = Math.Max(1, Math.Min(page, totalPages));

            var paginatedStudents = allStudents
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new StudentListVM
            {
                Students = paginatedStudents,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalStudents = totalStudents,
                SearchTerm = searchTerm
            };

            return View("ViewStudents", viewModel);
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

                    return RedirectToAction("ViewCourses");
                }
                else
                {
                    ViewBag.ErrorMessage = "There was an error adding the course.";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("ViewCourses");
            }
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
            var class_ = await _classRepo.GetClassByIdAsync(classId);

            if (class_ == null || class_.Instructor.Id != user.Id)
            {
                return NotFound();
            }

            var model = new AddAssignmentVM
            {
                ClassId = classId,
                ClassName = class_.ParentCourse.Name,
                Term = class_.Term,
                DueDate = DateTime.Now,
                ReviewDueDate = DateTime.Now.AddDays(7)
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
            var class_ = await _classRepo.GetClassByIdAsync(model.ClassId);

            if (class_ == null || class_.Instructor.Id != user.Id)
            {
                return NotFound();
            }

            var assignment = new Assignment
            {
                Title = model.Title,
                DueDate = model.DueDate,
                ReviewDueDate = model.ReviewDueDate,
                Course = class_.ParentCourse
            };

            await _assignmentRepo.AddAssignmentAsync(assignment);

            TempData["Message"] = "Assignment added successfully.";
            return RedirectToAction("ViewAssignments", new { classId = model.ClassId });
        }



        public async Task<IActionResult> ViewAssignments(int classId)
        {
            var user = await _userManager.GetUserAsync(User);
            var _class = await _classRepo.GetClassByIdAsync(classId);

            if (_class == null || _class.Instructor.Id != user.Id)
            {
                return NotFound();
            }

            var assignments = await _assignmentRepo.GetAssignmentsByCourseAsync(_class.ParentCourse.Id);

            ViewBag.ClassId = classId;
            ViewBag.ClassName = _class.ParentCourse.Name;
            ViewBag.Term = _class.Term;

            return View(assignments);
        }

        public async Task<IActionResult> UpcomingDeadlines()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var assignments = await _assignmentRepo.GetAssignmentsByInstructorAsync(currentUser);

            var dueVersions = assignments
                .Where(v => v.DueDate > DateTime.Now
                            && v.DueDate < DateTime.Now.AddDays(7))
                .ToList();

            return View(dueVersions);
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
            var assignment = await _assignmentRepo.GetAssignmentByIdAsync(assignmentId);
            var submissions = await _assignmentSubmissionRepo.GetSubmissionsByAssignmentAsync(assignmentId);

            ViewBag.AssignmentTitle = assignment.Title;
            ViewBag.AssignmentId = assignment.Id;
            ViewBag.DueDate = assignment.DueDate;
            ViewBag.ClassId = assignment.Course.Id;

            var vm = new ViewSubmissionsVM() { Submissions = submissions };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAssignmentGrade(int submissionId, int assignmentId)
        {
            //get submission that is being graded
            var updatedSubmission = await _assignmentSubmissionRepo.GetSubmissionByIdAsync(submissionId);

            var gradeStr = Request.Form[$"submissionGrade_{submissionId}"];

            if (int.TryParse(gradeStr, out int gradeValue))
            {
                // Save the grade
                var model = new Grade
                {
                    Value = gradeValue,
                    Student = updatedSubmission.Submitter
                };

                //add grade reference
                await _gradeRepo.AddGradeAsync(model);

                //update model
                updatedSubmission.AssignmentGrade = model;
            }

            await _assignmentSubmissionRepo.UpdateAssignmentSubmissionAsync(updatedSubmission);

            //redirect back to submission page
            return RedirectToAction("ViewSubmissions", new { assignmentId });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReviewGrade(Grade model, int id)
        {
            return View();
        }

        public async Task<IActionResult> AddStudents(int classId)
        {
            var user = await _userManager.GetUserAsync(User);
            var class_ = await _classRepo.GetClassByIdAsync(classId);

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


            await _classRepo.AddStudentsToClassAsync(model.ClassId, model.SelectedStudentIds);

            return RedirectToAction("ViewStudents");
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

        public async Task<IActionResult> EditAssignmentVersion(int versionId)
        {
            var version = await _assignmentVersionRepo.GetAssignmentVersionByIdAsync(versionId);
            
            // get appuser for current user
            var user = _userManager.GetUserAsync(User).Result;

            //Get list of Assignments and Documents to display for class
            var documents = await _documentRepo.GetDocumentsAsync(user.Id);
            var assignments = await _assignmentRepo.GetAssignmentsByInstructorAsync(user);
            
            var vm = new AssignmentVersionVM { Documents = documents, Assignments = assignments, AssnVersion = version };
            return View(vm);
        }
    
        [HttpPost]
        public async Task<IActionResult> EditAssignmentVersion(AssignmentVersionVM model)
        {
            await _assignmentVersionRepo.UpdateAssignmentVersionAsync(model.AssnVersion);

            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> DeleteAssignmentVersion(int versionId, int classId)
        {
            await _assignmentVersionRepo.DeleteAssignmentVersionAsync(versionId);
            
            return RedirectToAction("ViewAssignments", new {classId});
        }

        public async Task<IActionResult> ViewGroups(int classId)
        {
            Class cls = await _classRepo.GetClassByIdAsync(classId);
            IList<AppUser> students = cls.Students;
            IList<Assignment> assignments = cls.ParentCourse.Assignments;
            ViewGroupsVM vm = new ViewGroupsVM() { Assignments = assignments, Students = students, Class = cls };

            return View(vm);

        }

        public async Task<IActionResult> ViewAllGroups()
        {
            var user = await _userManager.GetUserAsync(User);
            var classes = await _classRepo.GetClassesForInstructorAsync(user);

            if (!classes.Any())
            {
                TempData["Message"] = "You don't have any classes yet. Create a class first.";
                return RedirectToAction("AddClass");
            }

            return View(classes);
        }

        public async Task<IActionResult> RemoveStudentFromGroup(string studentId, int assignmentId)
        {
            
            await _assignmentVersionRepo.DeleteStudentFromAssignmentVersionAsync(studentId, assignmentId);

            return RedirectToAction("ViewAllGroups");
        }

        public async Task<IActionResult> SortGroup(int classId, int assignmentId)
        {
            Class cls = await _classRepo.GetClassByIdAsync(classId);
            IList<AppUser> students = cls.Students;
            await _assignmentVersionRepo.DeleteStudentsFromAssignmentVersionAsync(students, assignmentId);
            await _assignmentVersionRepo.AddStudentsToAssignmentVersionsAsync(students, assignmentId);

            return RedirectToAction("ViewAllGroups");
        }


        public async Task<IActionResult> ReadyForGrading()
        {
            var user = await _userManager.GetUserAsync(User);
            var classes = await _classRepo.GetClassesForInstructorAsync(user);

            var viewModel = new ReadyForGradingVM();

            // Get ungraded assignment submissions
            foreach (var cls in classes)
            {
                foreach (var assignment in cls.ParentCourse.Assignments)
                {
                    // Get submissions for this assignment that have been reviewed but not graded
                    var ungradedSubmissions = await _assignmentSubmissionRepo.GetSubmissionsByAssignmentAsync(assignment.Id);
                    var submissionsReadyForGrading = ungradedSubmissions
                        .Where(s => s.Review != null && s.Review.ReviewDocument != null && s.AssignmentGrade == null)
                        .ToList();

                    if (submissionsReadyForGrading.Any())
                    {
                        viewModel.UnGradedSubmissions.Add(new UnGradedSubmissionGroup
                        {
                            Assignment = assignment,
                            Class = cls,
                            Submissions = submissionsReadyForGrading
                        });
                    }
                }
            }

            // Get ungraded reviews
            foreach (var cls in classes)
            {
                foreach (var assignment in cls.ParentCourse.Assignments)
                {
                    // Get all reviews for assignments in this instructor's classes that haven't been graded
                    var allSubmissions = await _assignmentSubmissionRepo.GetSubmissionsByAssignmentAsync(assignment.Id);
                    var ungradedReviews = allSubmissions
                        .Where(s => s.Review != null && s.Review.ReviewDocument != null && s.Review.ReviewGrade == null)
                        .Select(s => s.Review)
                        .ToList();

                    if (ungradedReviews.Any())
                    {
                        viewModel.UnGradedReviews.Add(new UnGradedReviewGroup
                        {
                            Assignment = assignment,
                            Class = cls,
                            Reviews = ungradedReviews
                        });
                    }
                }
            }

            // Calculate total items to grade
            viewModel.TotalItemsToGrade = viewModel.UnGradedSubmissions.Sum(g => g.Submissions.Count) +
                                           viewModel.UnGradedReviews.Sum(g => g.Reviews.Count);

            return View(viewModel);
        }

        // Helper method to get the count for the dashboard widget
        public async Task<int> GetReadyForGradingCountAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var classes = await _classRepo.GetClassesForInstructorAsync(user);

            int count = 0;

            foreach (var cls in classes)
            {
                foreach (var assignment in cls.ParentCourse.Assignments)
                {
                    var submissions = await _assignmentSubmissionRepo.GetSubmissionsByAssignmentAsync(assignment.Id);

                    // Count ungraded submissions that have been reviewed
                    count += submissions.Count(s => s.Review != null && s.Review.ReviewDocument != null && s.AssignmentGrade == null);

                    // Count ungraded reviews
                    count += submissions.Count(s => s.Review != null && s.Review.ReviewDocument != null && s.Review.ReviewGrade == null);
                }
            }

            return count;
        }


        [HttpPost]
        public async Task<IActionResult> SubmitReviewGrade(int reviewId, int gradeValue, string comments = "")
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);

            if (review == null)
            {
                return NotFound();
            }

            // Create the grade for the review
            var grade = new Grade
            {
                Value = gradeValue,
                Student = review.Reviewer // The reviewer gets graded on their review quality
            };

            await _gradeRepo.AddGradeAsync(grade);

            // Update the review with the grade
            review.ReviewGrade = grade;
            await _reviewRepository.UpdateReviewAsync(review);

            TempData["Message"] = $"Review grade submitted successfully for {review.Reviewer.UserName}.";

            return RedirectToAction("ReadyForGrading");
        }


       



    }
}

