using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PeerReviewApp.Controllers;
using PeerReviewApp.Data;
using PeerReviewApp.Models;
using Xunit;

namespace PeerReviewApp.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void CreateInstructor_GET_ReturnsView()
        {
            // Arrange
            var controller = new AdminController(null, null, null, null, null);

            // Act
            var result = controller.CreateInstructor();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreateInstitution_GET_ReturnsView()
        {
            // Arrange
            var controller = new AdminController(null, null, null, null, null);

            // Act
            var result = controller.CreateInstitution();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ManageInstitutions_ReturnsView()
        {
            // Arrange
            var mockInstitutionRepo = new Mock<IInstitutionRepository>();
            mockInstitutionRepo.Setup(x => x.GetInstitutionsAsync()).ReturnsAsync(new List<Institution>());
            var controller = new AdminController(null, null, null, mockInstitutionRepo.Object, null);

            // Act
            var result = controller.ManageInstitutions();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_ReturnsViewWithDashboardData()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockInstitutionRepo = new Mock<IInstitutionRepository>();
            var mockCourseRepo = new Mock<ICourseRepository>();

            mockInstitutionRepo.Setup(x => x.GetInstitutionsAsync()).ReturnsAsync(new List<Institution>());
            mockUserManager.Setup(x => x.GetUsersInRoleAsync("Instructor")).ReturnsAsync(new List<AppUser>());
            mockCourseRepo.Setup(x => x.GetCoursesAsync()).ReturnsAsync(new List<Course>());
            mockUserManager.Setup(x => x.Users).Returns(new List<AppUser>().AsQueryable());

            var controller = new AdminController(mockUserManager.Object, null, null, mockInstitutionRepo.Object, mockCourseRepo.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<AdminDashboardVM>(viewResult.Model);
        }

        [Fact]
        public async Task ManageInstructors_ReturnsViewWithInstructors()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            var instructorRole = new IdentityRole("Instructor");
            mockRoleManager.Setup(x => x.FindByNameAsync("Instructor")).ReturnsAsync(instructorRole);
            mockUserManager.Setup(x => x.GetUsersInRoleAsync("Instructor")).ReturnsAsync(new List<AppUser>());

            var controller = new AdminController(mockUserManager.Object, mockRoleManager.Object, null, null, null);

            // Act
            var result = await controller.ManageInstructors();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<AppUser>>(viewResult.Model);
        }

        [Fact]
        public async Task ManageStudents_WithSearchTerm_FiltersStudents()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            var studentRole = new IdentityRole("Student");
            var students = new List<AppUser>
    {
        new AppUser { UserName = "john", Email = "john@test.com" },
        new AppUser { UserName = "jane", Email = "jane@test.com" }
    };

            mockRoleManager.Setup(x => x.FindByNameAsync("Student")).ReturnsAsync(studentRole);
            mockUserManager.Setup(x => x.GetUsersInRoleAsync("Student")).ReturnsAsync(students);

            var controller = new AdminController(mockUserManager.Object, mockRoleManager.Object, null, null, null);

            // Act
            var result = await controller.ManageStudents("john");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<List<AppUser>>(viewResult.Model);
            Assert.Single(model);
            Assert.Equal("john", model.First().UserName);
        }

    }
}