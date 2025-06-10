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
    }
}