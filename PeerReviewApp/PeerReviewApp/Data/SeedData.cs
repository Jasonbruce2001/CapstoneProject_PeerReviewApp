using PeerReviewApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PeerReviewApp.Data;

public class SeedData
{
    public static async Task Seed(ApplicationDbContext context, IServiceProvider provider)
    {
        var userManager = provider
            .GetRequiredService<UserManager<AppUser>>();
        RoleManager<IdentityRole> roleManager =
            provider.GetRequiredService<RoleManager<IdentityRole>>();

        DateTime date = DateTime.Now;
        const string SECRET_PASSWORD = "Pass!123";

        //AppUser student = new AppUser { UserName = "Aiden", Email = "testMail@gmail.com", AccountAge = date };
        AppUser instructor = new AppUser { UserName = "Brian", Email = "testMail2@gmail.com", AccountAge = date };

        if (userManager.Users.ToList().Count == 0)
        {
            var result = await userManager.CreateAsync(instructor, SECRET_PASSWORD);
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(instructor, "Instructor");
            }
            
            //create student role if doesn't exist
            if (await roleManager.FindByNameAsync("Student") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Student"));
            }

            // Adding a whole class of students
            AppUser student1 = new AppUser { UserName = "User1", Email = "testMail1@gmail.com", AccountAge = date };
            AppUser student2 = new AppUser { UserName = "User2", Email = "testMail2@gmail.com", AccountAge = date };
            AppUser student3 = new AppUser { UserName = "User3", Email = "testMail3@gmail.com", AccountAge = date };
            AppUser student4 = new AppUser { UserName = "User4", Email = "testMail4@gmail.com", AccountAge = date };
            AppUser student5 = new AppUser { UserName = "User5", Email = "testMail5@gmail.com", AccountAge = date };
            AppUser student6 = new AppUser { UserName = "User6", Email = "testMail6@gmail.com", AccountAge = date };
            AppUser student7 = new AppUser { UserName = "User7", Email = "testMail7@gmail.com", AccountAge = date };
            AppUser student8 = new AppUser { UserName = "User8", Email = "testMail8@gmail.com", AccountAge = date };
            AppUser student9 = new AppUser { UserName = "User9", Email = "testMail9@gmail.com", AccountAge = date };
            AppUser student10 = new AppUser { UserName = "User10", Email = "testMail10@gmail.com", AccountAge = date };
            AppUser student11 = new AppUser { UserName = "User11", Email = "testMail11@gmail.com", AccountAge = date };
            AppUser student12 = new AppUser { UserName = "User12", Email = "testMail12@gmail.com", AccountAge = date };
            AppUser student13 = new AppUser { UserName = "User13", Email = "testMail13@gmail.com", AccountAge = date };
            AppUser student14 = new AppUser { UserName = "User14", Email = "testMail14@gmail.com", AccountAge = date };
            AppUser student15 = new AppUser { UserName = "User15", Email = "testMail15@gmail.com", AccountAge = date };
            AppUser student16 = new AppUser { UserName = "User16", Email = "testMail16@gmail.com", AccountAge = date };
            AppUser student17 = new AppUser { UserName = "User17", Email = "testMail17@gmail.com", AccountAge = date };
            AppUser student18 = new AppUser { UserName = "User18", Email = "testMail18@gmail.com", AccountAge = date };
            AppUser student19 = new AppUser { UserName = "User19", Email = "testMail19@gmail.com", AccountAge = date };
            AppUser student20 = new AppUser { UserName = "User20", Email = "testMail20@gmail.com", AccountAge = date };
            AppUser student21 = new AppUser { UserName = "User21", Email = "testMail21@gmail.com", AccountAge = date };
            AppUser student22 = new AppUser { UserName = "User22", Email = "testMail22@gmail.com", AccountAge = date };
            

            await userManager.CreateAsync(student1, SECRET_PASSWORD);
            await userManager.CreateAsync(student2, SECRET_PASSWORD);
            await userManager.CreateAsync(student3, SECRET_PASSWORD);
            await userManager.CreateAsync(student4, SECRET_PASSWORD);
            await userManager.CreateAsync(student5, SECRET_PASSWORD);
            await userManager.CreateAsync(student6, SECRET_PASSWORD);
            await userManager.CreateAsync(student7, SECRET_PASSWORD);
            await userManager.CreateAsync(student8, SECRET_PASSWORD);
            await userManager.CreateAsync(student9, SECRET_PASSWORD);
            await userManager.CreateAsync(student10, SECRET_PASSWORD);
            await userManager.CreateAsync(student11, SECRET_PASSWORD);
            await userManager.CreateAsync(student12, SECRET_PASSWORD);
            await userManager.CreateAsync(student13, SECRET_PASSWORD);
            await userManager.CreateAsync(student14, SECRET_PASSWORD);
            await userManager.CreateAsync(student15, SECRET_PASSWORD);
            await userManager.CreateAsync(student16, SECRET_PASSWORD);
            await userManager.CreateAsync(student17, SECRET_PASSWORD);
            await userManager.CreateAsync(student18, SECRET_PASSWORD);
            await userManager.CreateAsync(student19, SECRET_PASSWORD);
            await userManager.CreateAsync(student20, SECRET_PASSWORD);
            await userManager.CreateAsync(student21, SECRET_PASSWORD);
            await userManager.CreateAsync(student22, SECRET_PASSWORD);


            IList<AppUser> students = new List<AppUser> { student1, student2, student3, student4, student5, student6, student7, student8, student9, student10, student11, student12, student13, student14, student15, student16, student17, student18, student19, student20, student21, student22};

            foreach (var s in students)
            {
                if (s.NormalizedUserName != null)
                {
                    await userManager.AddToRoleAsync(s, "Student");
                }
            }


            //adding instructors seed data 
            AppUser instructor1 = new AppUser { UserName = "Instructor1", Email = "instructor1@mail.com", AccountAge = date };
            AppUser instructor2 = new AppUser { UserName = "Instructor2", Email = "instructor2@mail.com", AccountAge = date };
            AppUser instructor3 = new AppUser { UserName = "Instructor3", Email = "instructor3@mail.com", AccountAge = date };
            AppUser instructor4 = new AppUser { UserName = "Instructor4", Email = "instructor4@mail.com", AccountAge = date };
            AppUser instructor5 = new AppUser { UserName = "Instructor5", Email = "instructor5@mail.com", AccountAge = date };
            AppUser instructor6 = new AppUser { UserName = "Instructor6", Email = "instructor6@mail.com", AccountAge = date };
            AppUser instructor7 = new AppUser { UserName = "Instructor7", Email = "instructor7@mail.com", AccountAge = date };
            AppUser instructor8 = new AppUser { UserName = "Instructor8", Email = "instructor8@mail.com", AccountAge = date };
            AppUser instructor9 = new AppUser { UserName = "Instructor9", Email = "instructor9@mail.com", AccountAge = date };
            AppUser instructor10 = new AppUser { UserName = "Instructor10", Email = "instructor10@mail.com", AccountAge = date };
            AppUser instructor11 = new AppUser { UserName = "Instructor11", Email = "instructor11@mail.com", AccountAge = date };
            AppUser instructor12 = new AppUser { UserName = "Instructor12", Email = "instructor12@mail.com", AccountAge = date };
            AppUser instructor13 = new AppUser { UserName = "Instructor13", Email = "instructor13@mail.com", AccountAge = date };
            AppUser instructor14 = new AppUser { UserName = "Instructor14", Email = "instructor14@mail.com", AccountAge = date };
            AppUser instructor15 = new AppUser { UserName = "Instructor15", Email = "instructor15@mail.com", AccountAge = date };
            AppUser instructor16 = new AppUser { UserName = "Instructor16", Email = "instructor16@mail.com", AccountAge = date };
            AppUser instructor17 = new AppUser { UserName = "Instructor17", Email = "instructor17@mail.com", AccountAge = date };
            AppUser instructor18 = new AppUser { UserName = "Instructor18", Email = "instructor18@mail.com", AccountAge = date };
            AppUser instructor19 = new AppUser { UserName = "Instructor19", Email = "instructor19@mail.com", AccountAge = date };
            AppUser instructor20 = new AppUser { UserName = "Instructor20", Email = "instructor20@mail.com", AccountAge = date };

            await userManager.CreateAsync(instructor1, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor2, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor3, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor4, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor5, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor6, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor7, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor8, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor9, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor10, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor11, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor12, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor13, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor14, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor15, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor16, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor17, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor18, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor19, SECRET_PASSWORD);
            await userManager.CreateAsync(instructor20, SECRET_PASSWORD);
            
            await userManager.AddToRoleAsync(instructor1, "Instructor");
            await userManager.AddToRoleAsync(instructor2, "Instructor");
            await userManager.AddToRoleAsync(instructor3, "Instructor");
            await userManager.AddToRoleAsync(instructor4, "Instructor");

            
            Institution inst = new Institution() { Name = "Institute", Code = "ABC123" };
            context.Institutions.Add(inst);
            
            Course course = new Course() { Name = "Test Course", Institution = inst, Description = "Test Description" };
            context.Courses.Add(course);
            Class class1 = new Class() { Instructor = instructor1, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class2 = new Class() { Instructor = instructor2, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class3 = new Class() { Instructor = instructor3, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class4 = new Class() { Instructor = instructor4, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class5 = new Class() { Instructor = instructor5, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class6 = new Class() { Instructor = instructor6, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class7 = new Class() { Instructor = instructor7, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class8 = new Class() { Instructor = instructor8, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class9 = new Class() { Instructor = instructor9, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class10 = new Class() { Instructor = instructor10, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class11 = new Class() { Instructor = instructor11, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class12 = new Class() { Instructor = instructor12, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class13 = new Class() { Instructor = instructor13, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class14 = new Class() { Instructor = instructor14, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class15 = new Class() { Instructor = instructor15, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class16 = new Class() { Instructor = instructor16, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class17 = new Class() { Instructor = instructor17, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class18 = new Class() { Instructor = instructor18, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class19 = new Class() { Instructor = instructor19, ParentCourse = course, Students = students, Term = "Spring 2025" };
            Class class20 = new Class() { Instructor = instructor20, ParentCourse = course, Students = students, Term = "Spring 2025" };

            context.Classes.Add(class1);
            context.Classes.Add(class2);
            context.Classes.Add(class3);
            context.Classes.Add(class4);
            context.Classes.Add(class5);
            context.Classes.Add(class6);
            context.Classes.Add(class7);
            context.Classes.Add(class8);
            context.Classes.Add(class9);
            context.Classes.Add(class10);
            context.Classes.Add(class11);
            context.Classes.Add(class12);
            context.Classes.Add(class13);
            context.Classes.Add(class14);
            context.Classes.Add(class15);
            context.Classes.Add(class16);
            context.Classes.Add(class17);
            context.Classes.Add(class18);
            context.Classes.Add(class19);
            context.Classes.Add(class20);

            Document doc1 = new Document() { Uploader = instructor1, Name = "Instructions1", FilePath = "SampleLabInstructions.pdf", FileSize = "1kb", DateUploaded = DateTime.Now};
            Document doc2 = new Document() { Uploader = instructor1, Name = "ReviewForm1", FilePath = "SampleReviewForm.pdf", FileSize = "1kb", DateUploaded = DateTime.Now };

            context.Documents.Add(doc1);
            context.Documents.Add(doc2);

            Assignment assignment1 = new Assignment() {Course = course, DueDate = DateTime.Now, Title = "Lab 1" };
            Assignment assignment2 = new Assignment() { Course = course, DueDate = DateTime.Now.AddDays(4), Title = "Lab 2" };
            Assignment assignment3 = new Assignment() { Course = course, DueDate = DateTime.Now.AddDays(11), Title = "Lab 3" };
            Assignment assignment4 = new Assignment() { Course = course, DueDate = DateTime.Now.AddDays(19), Title = "Lab 4" };

            context.Assignments.Add(assignment1);
            
            Grade grade1 = new Grade() { Value = 94, Student = student1 };
            Grade grade2 = new Grade() { Value = 84, Student = student1 };
            Grade grade3 = new Grade() { Value = 79, Student = student1 };
            Grade grade4 = new Grade() { Value = 100, Student = student1 };

            context.Grades.Add(grade1);
            context.Grades.Add(grade2);
            context.Grades.Add(grade3);
            context.Grades.Add(grade4);

            AssignmentVersion assignmentVersion1 = new AssignmentVersion() { ParentAssignment = assignment1, Name = "Version 1", TextInstructions = "Instructions for things", Instructions = doc1, ReviewForm = doc2, Students = { student1, student2, student3, student4, student5 } };
            AssignmentVersion assignmentVersion2 = new AssignmentVersion() { ParentAssignment = assignment1, Name = "Version 2", TextInstructions = "Instructions for things", Instructions = doc1, ReviewForm = doc2, Students = { student6, student7, student8, student9, student10 } };
            AssignmentVersion assignmentVersion3 = new AssignmentVersion() { ParentAssignment = assignment1, Name = "Version 3", TextInstructions = "Instructions for things", Instructions = doc1, ReviewForm = doc2, Students = { student11, student12, student13, student14, student15, } };
            AssignmentVersion assignmentVersion4 = new AssignmentVersion() { ParentAssignment = assignment1, Name = "Version 4", TextInstructions = "Instructions for things", Instructions = doc1, ReviewForm = doc2, Students = { student16, student17, student18, student19, student20, } };
            AssignmentVersion assignment2Version1 = new AssignmentVersion() { ParentAssignment = assignment2, Name = "Version 1", TextInstructions = "Instructions for things", Instructions = doc1, ReviewForm = doc2, Students = { student1, student2, student3, student4, student5, student6, student7, student8, student9, student10, student11, student12, student13, student14, student15, student16, student17, student18, student19, student20 } };
            AssignmentVersion assignment3Version1 = new AssignmentVersion() { ParentAssignment = assignment3, Name = "Version 1", TextInstructions = "Instructions for things", Instructions = doc1, ReviewForm = doc2, Students = { student1, student2, student3, student4, student5, student6, student7, student8, student9, student10, student11, student12, student13, student14, student15, student16, student17, student18, student19, student20 } };
            AssignmentVersion assignment4Version1 = new AssignmentVersion() { ParentAssignment = assignment4, Name = "Version 1", TextInstructions = "Instructions for things", Instructions = doc1, ReviewForm = doc2, Students = { student1, student2, student3, student4, student5, student6, student7, student8, student9, student10, student11, student12, student13, student14, student15, student16, student17, student18, student19, student20 } };

            context.AssignmentVersions.Add(assignmentVersion1);
            context.AssignmentVersions.Add(assignmentVersion2);
            context.AssignmentVersions.Add(assignmentVersion3);
            context.AssignmentVersions.Add(assignmentVersion4);
            context.AssignmentVersions.Add(assignment2Version1);
            context.AssignmentVersions.Add(assignment3Version1);
            context.AssignmentVersions.Add(assignment4Version1);

            Review review1 = new Review() { Reviewee = student1, Reviewer = student2, ReviewDocument = doc2, ReviewGrade = grade1};
            Review review2 = new Review() { Reviewee = student1, Reviewer = student3, ReviewDocument = doc2, ReviewGrade = grade2};
            Review review3 = new Review() { Reviewee = student2, Reviewer = student1, ReviewDocument = doc2, ReviewGrade = grade3};
            Review review4 = new Review() { Reviewee = student2, Reviewer = student3, ReviewDocument = doc2, ReviewGrade = grade4};
            Review review5 = new Review() { Reviewee = student3, Reviewer = student1, ReviewDocument = doc2, ReviewGrade = grade2};
            Review review6 = new Review() { Reviewee = student3, Reviewer = student2, ReviewDocument = doc2, ReviewGrade = grade4};

            context.Reviews.Add(review1);
            context.Reviews.Add(review2);
            context.Reviews.Add(review3);
            context.Reviews.Add(review4);
            context.Reviews.Add(review5);
            context.Reviews.Add(review6);

            /*
            AssignmentSubmission submission1 = new AssignmentSubmission() {AssignmentLink = "github.com", AssignmentVersion = assignmentVersion1, Review = review1, SubmissionDate = DateTime.Now, Submitter = student1 };
            AssignmentSubmission submission2 = new AssignmentSubmission() { AssignmentLink = "github.com", AssignmentVersion = assignmentVersion1, Review = review2, SubmissionDate = DateTime.Now, Submitter = student1 };
            AssignmentSubmission submission3 = new AssignmentSubmission() { AssignmentLink = "github.com", AssignmentVersion = assignmentVersion1, Review = review3, SubmissionDate = DateTime.Now, Submitter = student2 };
            AssignmentSubmission submission4 = new AssignmentSubmission() { AssignmentLink = "github.com", AssignmentVersion = assignmentVersion1, Review = review4, SubmissionDate = DateTime.Now, Submitter = student2 };
            AssignmentSubmission submission5 = new AssignmentSubmission() { AssignmentLink = "github.com", AssignmentVersion = assignmentVersion1, Review = review5, SubmissionDate = DateTime.Now, Submitter = student3 };
            AssignmentSubmission submission6 = new AssignmentSubmission() { AssignmentLink = "github.com", AssignmentVersion = assignmentVersion1, Review = review6, SubmissionDate = DateTime.Now, Submitter = student3 };
            
            
            context.AssignmentSubmissions.Add(submission1);
            context.AssignmentSubmissions.Add(submission2);
            context.AssignmentSubmissions.Add(submission3);
            context.AssignmentSubmissions.Add(submission4);
            context.AssignmentSubmissions.Add(submission5);
            context.AssignmentSubmissions.Add(submission6);*/

            context.Grades.Add(grade1);
            context.Grades.Add(grade2);
            context.Grades.Add(grade3);
            context.Grades.Add(grade4);

            IList<Class> classes = new List<Class> { class1, class2, class3 };
            
            AppUser listStudent = new AppUser { UserName = "RelationshipTest", Email = "UniqueEmail@gmail.com", AccountAge = date };
            await userManager.CreateAsync(listStudent, SECRET_PASSWORD);
            
            await context.SaveChangesAsync();
        }
    }

    //create admin user
    public static async Task CreateAdminUser(IServiceProvider serviceProvider)
    {
        UserManager<AppUser> userManager =
            serviceProvider.GetRequiredService<UserManager<AppUser>>();
        RoleManager<IdentityRole> roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string username = "Administrator";
        string email = "admin@admin.com";
        string password = "Pass1234!";
        string roleName = "Admin";
        DateTime date = DateTime.Now;

        // if role doesn't exist, create it
        if (await roleManager.FindByNameAsync(roleName) == null)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
        // if username doesn't exist, create it and add it to role
        if (await userManager.FindByNameAsync(username) == null)
        {
            AppUser user = new AppUser { UserName = username, AccountAge = date, Email = email };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}

/*
        var userManager = provider
            .GetRequiredService<UserManager<AppUser>>();

        //only add users if there are none
        if (!userManager.Users.Any())
        {
            // Create User objects
            DateTime date = DateTime.Now;
            const string SECRET_PASSWORD = "Pass!123";

            AppUser testUser1 = new AppUser { UserName = "Aiden", AccountAge = date };
            await userManager.CreateAsync(testUser1, SECRET_PASSWORD);

            AppUser testUser2 = new AppUser { UserName = "Jason", AccountAge = date };
            await userManager.CreateAsync(testUser2, SECRET_PASSWORD);

            AppUser testUser3 = new AppUser { UserName = "Travis", AccountAge = date };
            await userManager.CreateAsync(testUser3, SECRET_PASSWORD);

            AppUser instructor = new AppUser { UserName = "Instructor", AccountAge = date };
            await userManager.CreateAsync(instructor, SECRET_PASSWORD);

            AppUser reviewer = new AppUser { UserName = "reviewer", AccountAge = date };
            await userManager.CreateAsync(reviewer, SECRET_PASSWORD);

            AppUser reviewee = new AppUser { UserName = "reviewee", AccountAge = date };
            await userManager.CreateAsync(reviewee, SECRET_PASSWORD);

            await context.SaveChangesAsync();
        }

        //Seeded Institutions 
        if (!context.Institution.Any())
        {
            context.Institution.AddRange(
                new Institution { Name = "Lane Community College" },
                new Institution { Name = "University Of Oregon" },
                new Institution { Name = "Linn Benton College" }
            );
            await context.SaveChangesAsync();
        }

        // Seed Courses
        if (!context.Courses.Any())
        {
            var user = await userManager.Users.FirstOrDefaultAsync();
            var instructor = await userManager.FindByNameAsync("Instructor");
            var actualInstructor = instructor ?? user;
            var institution = context.Institution.FirstOrDefault();

            if (actualInstructor != null)
            {
                context.Courses.AddRange(
                    new Course
                    {
                        Name = "CS 246: System Design",
                        Instructor = actualInstructor,
                        InstructorId = actualInstructor.Id,
                        Term = "Winter 2025",
                        Students = new List<AppUser>()
                    },
                    new Course
                    {
                        Name = "CS 233: Programming Concepts",
                        Instructor = actualInstructor,
                        InstructorId = actualInstructor.Id,
                        Term = "Spring 2025",
                        Students = new List<AppUser>()
                    },
                    new Course
                    {
                        Name = "CS 275: Database Management",
                        Instructor = actualInstructor,
                        InstructorId = actualInstructor.Id,
                        Term = "Spring 2025",
                        Students = new List<AppUser>()
                    },
                    new Course
                    {
                        Name = "Course 1",
                        Instructor = actualInstructor,
                        InstructorId = actualInstructor.Id,
                        Term = "Spring",
                        Students = new List<AppUser>()
                    }
                );
                await context.SaveChangesAsync();
            }
        }

        // Seed Documents
        if (!context.Document.Any())
        {
            var user = await userManager.Users.FirstOrDefaultAsync();

            if (user != null)
            {
                context.Document.AddRange(
                    new Document
                    {
                        Uploader = user,
                        UploaderId = user.Id,
                        FilePath = "/uploads/syllabus.pdf"
                    },
                    new Document
                    {
                        Uploader = user,
                        UploaderId = user.Id,
                        FilePath = "/uploads/lecture_notes.docx"
                    },
                    new Document
                    {
                        Uploader = user,
                        UploaderId = user.Id,
                        FilePath = "/uploads/assignmentInstruction.pdf"
                    }
                );
                await context.SaveChangesAsync();
            }
        }

        // Seed Assignments
        if (!context.Assignments.Any())
        {
            var user = await userManager.Users.FirstOrDefaultAsync();
            var reviewer = await userManager.FindByNameAsync("reviewer");
            var actualReviewer = reviewer ?? user;
            var courses = await context.Courses.ToListAsync();

            if (user != null && courses.Any())
            {
                // First set from Aiden's branch
                context.Assignments.AddRange(
                    new Assignment
                    {
                        CourseId = courses[0].Id.ToString(),
                        DueDate = DateTime.Parse("11/21/2024"),
                        Title = "Lab 1",
                        Description = "Make a Project",
                        FilePath = "/uploads/assignmentInstruction.pdf"
                    },
                    new Assignment
                    {
                        CourseId = courses[0].Id.ToString(),
                        DueDate = DateTime.Parse("11/28/2024"),
                        Title = "Lab 2",
                        Description = "Make a Project Again",
                        FilePath = "/uploads/assignmentInstruction2.pdf"
                    },
                    new Assignment
                    {
                        CourseId = courses[0].Id.ToString(),
                        DueDate = DateTime.Parse("12/05/2024"),
                        Title = "Lab 3",
                        Description = "Make a Third Project",
                        FilePath = "/uploads/assignmentInstruction3.pdf"
                    }
                );

                // Create separate objects for the second set of assignments
                // Use a different course if available
                var coursesCount = courses.Count;
                var courseForSet2 = coursesCount > 3 ? courses[3] : courses[0]; // Use "Course 1" if available

                DateTime date = DateTime.Now;

                var assignment1 = new Assignment()
                {
                    DueDate = date,
                    CourseId = courseForSet2.Id.ToString(),
                    Title = "Test Assignment",
                    Description = "Test Description",
                    FilePath = "/Assignments",
                };

                var assignment2 = new Assignment()
                {
                    DueDate = date,
                    CourseId = courseForSet2.Id.ToString(),
                    Title = "Test Assignment 2",
                    Description = "Test Description 2",
                    FilePath = "/Assignments",
                };

                var assignment3 = new Assignment()
                {
                    DueDate = date,
                    CourseId = courseForSet2.Id.ToString(),
                    Title = "Test Assignment 3",
                    Description = "Test Description 3",
                    FilePath = "/Assignments",
                };

                context.Assignments.Add(assignment1);
                context.Assignments.Add(assignment2);
                context.Assignments.Add(assignment3);

                await context.SaveChangesAsync();

                // Now create reviews for the second set of assignments
                var reviewee = await userManager.FindByNameAsync("reviewee");
                var actualReviewee = reviewee ?? user;

                if (actualReviewer != null && actualReviewee != null)
                {
                    // Note: Review model has private DueDate and FilePath
                    // If this causes issues, you'll need to modify the Review model

                    var review1 = new Review()
                    {
                        AssignmentId = assignment1.Id,
                        Reviewer = actualReviewer,
                        Reviewee = actualReviewee
                        // Can't set DueDate and FilePath as they're private
                    };

                    var review2 = new Review()
                    {
                        AssignmentId = assignment2.Id,
                        Reviewer = actualReviewer,
                        Reviewee = actualReviewee
                        // Can't set DueDate and FilePath as they're private
                    };

                    var review3 = new Review()
                    {
                        AssignmentId = assignment3.Id,
                        Reviewer = actualReviewer,
                        Reviewee = actualReviewee
                        // Can't set DueDate and FilePath as they're private
                    };

                    context.Reviews.Add(review1);
                    context.Reviews.Add(review2);
                    context.Reviews.Add(review3);

                    await context.SaveChangesAsync();
                }
            }
        }

        // Seed Grades
        if (!context.Grade.Any())
        {
            var user = await userManager.Users.FirstOrDefaultAsync();
            var assignments = await context.Assignments.ToListAsync();

            if (user != null && assignments.Count >= 3)
            {
                context.Grade.AddRange(
                    new Grade
                    {
                        Student = user,
                        AssignmentId = assignments[0].Id,
                        Value = 94
                    },
                    new Grade
                    {
                        Student = user,
                        AssignmentId = assignments[1].Id,
                        Value = 81
                    },
                    new Grade
                    {
                        Student = user,
                        AssignmentId = assignments[2].Id,
                        Value = 54
                    }
                );
                await context.SaveChangesAsync();
            }
        }

        // Seed Group
        if (!context.Group.Any())
        {
            var courses = await context.Courses.ToListAsync();

            if (courses.Any())
            {
                context.Group.AddRange(
                    new Group
                    {
                        Name = "Group 1",
                        CourseId = courses[0].Id
                    },
                    new Group
                    {
                        Name = "Group 2",
                        CourseId = courses[0].Id
                    },
                    new Group
                    {
                        Name = "Group 3",
                        CourseId = courses[0].Id
                    }
                );
                await context.SaveChangesAsync();
            }
        }

        // Seed GroupMembers
        if (!context.GroupMembers.Any())
        {
            var user = await userManager.Users.FirstOrDefaultAsync();
            var groups = await context.Group.ToListAsync();

            if (user != null && groups.Any())
            {
                context.GroupMembers.AddRange(
                    new GroupMembers
                    {
                        Group = groups[0],
                        Member = user
                    },
                    new GroupMembers
                    {
                        Group = groups.Count > 1 ? groups[1] : groups[0],
                        Member = user
                    },
                    new GroupMembers
                    {
                        Group = groups.Count > 2 ? groups[2] : groups[0],
                        Member = user
                    }
                );
                await context.SaveChangesAsync();
            }
        }*/