using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PeerReviewApp.Controllers;
using PeerReviewApp.Data;
using PeerReviewApp.Models;
using System.Security.Claims;
using Xunit;

namespace PeerReviewApp.Tests
{
    public class InstructorControllerTests
    {
        [Fact]
        public void AddCourse_GET_ReturnsView()
        {
            // Arrange
            var mockInstitutionRepo = new Mock<IInstitutionRepository>();
            mockInstitutionRepo.Setup(x => x.GetInstitutionsAsync()).ReturnsAsync(new List<Institution>());
            var controller = new InstructorController(null, null, null, mockInstitutionRepo.Object, null, null, null, null, null, null, null, null);

            // Act
            var result = controller.AddCourse();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async Task Index_ReturnsViewWithInstructorDashboard()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<AppUser>>(
                mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(), null, null, null, null);
            var mockClassRepo = new Mock<IClassRepository>();

            var user = new AppUser { Id = "test-id" };
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockSignInManager.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);
            mockClassRepo.Setup(x => x.GetCoursesForInstructorAsync(user)).ReturnsAsync(new List<Course>());
            mockClassRepo.Setup(x => x.GetClassesForInstructorAsync(user)).ReturnsAsync(new List<Class>());

            var controller = new InstructorController(null, mockUserManager.Object, null, null, mockClassRepo.Object, mockSignInManager.Object, null, null, null, null, null, null);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<InstructorDashVM>(viewResult.Model);
        }

        [Fact]
        public async Task AddAssignment_GET_ReturnsViewWithModel()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockClassRepo = new Mock<IClassRepository>();

            var user = new AppUser { Id = "instructor-id" };
            var course = new Course { Name = "Test Course" };
            var testClass = new Class { ClassId = 1, ParentCourse = course, Term = "Spring 2025", Instructor = user };

            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockClassRepo.Setup(x => x.GetClassByIdAsync(1)).ReturnsAsync(testClass);

            var controller = new InstructorController(null, mockUserManager.Object, null, null, mockClassRepo.Object, null, null, null, null, null, null, null);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await controller.AddAssignment(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AddAssignmentVM>(viewResult.Model);
            Assert.Equal(1, model.ClassId);
            Assert.Equal("Test Course", model.ClassName);
        }

        [Fact]
        public async Task ViewAssignments_ReturnsViewWithAssignments()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockClassRepo = new Mock<IClassRepository>();
            var mockAssignmentRepo = new Mock<IAssignmentRepository>();

            var user = new AppUser { Id = "instructor-id" };
            var course = new Course { Id = 1, Name = "Test Course" };
            var testClass = new Class { ClassId = 1, ParentCourse = course, Instructor = user };

            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockClassRepo.Setup(x => x.GetClassByIdAsync(1)).ReturnsAsync(testClass);
            mockAssignmentRepo.Setup(x => x.GetAssignmentsByCourseAsync(1)).ReturnsAsync(new List<Assignment>());

            var controller = new InstructorController(null, mockUserManager.Object, null, null, mockClassRepo.Object, null, null, mockAssignmentRepo.Object, null, null, null, null);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await controller.ViewAssignments(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

    }
}