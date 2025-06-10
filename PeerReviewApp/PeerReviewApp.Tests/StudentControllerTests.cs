using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PeerReviewApp.Controllers;
using PeerReviewApp.Data;
using PeerReviewApp.Models;
using PeerReviewApp.Models.ViewModels;
using System.Security.Claims;
using Xunit;

namespace PeerReviewApp.Tests
{
    public class StudentControllerTests
    {
        [Fact]
        public async Task Assignments_ReturnsView()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockAssignmentVersionRepo = new Mock<IAssignmentVersionRepository>();

            var testUser = new AppUser { Id = "test-id" };
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(testUser);
            mockAssignmentVersionRepo.Setup(x => x.GetAssignmentVersionsForStudentAsync(testUser)).ReturnsAsync(new List<AssignmentVersion>());

            var controller = new StudentController(null, null, null, null, mockUserManager.Object, mockAssignmentVersionRepo.Object, null, null);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await controller.Assignments();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_ReturnsViewWithStudentDashboard()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockClassRepo = new Mock<IClassRepository>();
            var mockSubmissionRepo = new Mock<IAssignmentSubmissionRepository>();

            var user = new AppUser { Id = "student-id" };
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockClassRepo.Setup(x => x.GetClassesForStudentAsync(user)).ReturnsAsync(new List<Class>());
            mockSubmissionRepo.Setup(x => x.GetSubmissionsByReviewerAsync(user)).ReturnsAsync(new List<AssignmentSubmission>());
            mockSubmissionRepo.Setup(x => x.GetAllSubmissionsByStudentAsync(user)).ReturnsAsync(new List<AssignmentSubmission>());

            var controller = new StudentController(mockClassRepo.Object, null, null, null, mockUserManager.Object, null, null, mockSubmissionRepo.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<StudentDashVM>(viewResult.Model);
        }

        [Fact]
        public async Task DetailedAssignment_WithValidId_ReturnsViewWithModel()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockAssignmentVersionRepo = new Mock<IAssignmentVersionRepository>();

            var user = new AppUser { Id = "student-id" };
            var assignment = new AssignmentVersion { Id = 1, Submissions = new List<AssignmentSubmission>() };

            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockAssignmentVersionRepo.Setup(x => x.GetAssignmentVersionByIdAsync(1)).ReturnsAsync(assignment);

            var controller = new StudentController(null, null, null, null, mockUserManager.Object, mockAssignmentVersionRepo.Object, null, null);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await controller.DetailedAssignment(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<DetailedAssignmentVM>(viewResult.Model);
            Assert.Equal(assignment, model.Assignment);
        }

        [Fact]
        public async Task ViewReviews_ReturnsViewWithReviews()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockSubmissionRepo = new Mock<IAssignmentSubmissionRepository>();

            var user = new AppUser { Id = "student-id" };
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockSubmissionRepo.Setup(x => x.GetSubmissionsByReviewerAsync(user)).ReturnsAsync(new List<AssignmentSubmission>());

            var controller = new StudentController(null, null, null, null, mockUserManager.Object, null, null, mockSubmissionRepo.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await controller.ViewReviews();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<AssignmentSubmission>>(viewResult.Model);
        }

        [Fact]
        public async Task DueSoon_ReturnsViewWithUpcomingAssignments()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockAssignmentVersionRepo = new Mock<IAssignmentVersionRepository>();

            var user = new AppUser { Id = "student-id" };
            var upcomingAssignment = new AssignmentVersion
            {
                ParentAssignment = new Assignment { DueDate = DateTime.Now.AddDays(3) }
            };
            var assignments = new List<AssignmentVersion> { upcomingAssignment };

            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockAssignmentVersionRepo.Setup(x => x.GetAssignmentVersionsForStudentAsync(user)).ReturnsAsync(assignments);

            var controller = new StudentController(null, null, null, null, mockUserManager.Object, mockAssignmentVersionRepo.Object, null, null);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await controller.DueSoon();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<List<AssignmentVersion>>(viewResult.Model);
            Assert.Single(model);
        }


    }
}