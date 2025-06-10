using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PeerReviewApp.Controllers;
using PeerReviewApp.Models;
using Xunit;

namespace PeerReviewApp.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsView()
        {
            // Arrange
            var controller = new HomeController(null, null, null);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upload_GET_ReturnsView()
        {
            // Arrange
            var controller = new HomeController(null, null, null);

            // Act
            var result = controller.Upload();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsView()
        {
            // Arrange
            var controller = new HomeController(null, null, null);

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsView()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(mockLogger.Object, null, null);

            // Mock HttpContext
            var httpContext = new DefaultHttpContext();
            httpContext.TraceIdentifier = "test-trace-id";
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.Equal("test-trace-id", model.RequestId);
        }

        [Fact]
        public void RelTesting_ReturnsView()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

            // Setup Users property to return empty list
            var users = new List<AppUser>().AsQueryable();
            mockUserManager.Setup(x => x.Users).Returns(users);

            var controller = new HomeController(mockLogger.Object, mockUserManager.Object, null);

            // Act
            var result = controller.RelTesting();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}