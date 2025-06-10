using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PeerReviewApp.Controllers;
using PeerReviewApp.Data;
using PeerReviewApp.Models;
using System.Security.Claims;
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

        [Fact]
        public async Task Documents_ReturnsViewWithUserDocuments()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockDocumentRepo = new Mock<IDocumentRepository>();

            var user = new AppUser { Id = "user-id" };
            var documents = new List<Document> { new Document { Name = "Test Doc" } };

            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockDocumentRepo.Setup(x => x.GetDocumentsByUserAsync(user)).ReturnsAsync(documents);

            var controller = new HomeController(null, mockUserManager.Object, mockDocumentRepo.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await controller.Documents();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<Document>>(viewResult.Model);
        }

        [Fact]
        public async Task DeleteUpload_WithValidId_RedirectsToDocuments()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockDocumentRepo = new Mock<IDocumentRepository>();
            var document = new Document { Id = 1, FilePath = "test.pdf" };

            mockDocumentRepo.Setup(x => x.GetDocumentByIdAsync(1)).ReturnsAsync(document);

            var controller = new HomeController(mockLogger.Object, null, mockDocumentRepo.Object);

            // Act
            var result = await controller.DeleteUpload(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Documents", redirectResult.ActionName);
        }
    }
}