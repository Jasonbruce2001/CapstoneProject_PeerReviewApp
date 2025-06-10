using Microsoft.AspNetCore.Mvc;
using PeerReviewApp.Controllers;
using PeerReviewApp.Models;
using Xunit;

namespace PeerReviewApp.Tests
{
    public class AccountControllerTests
    {
        [Fact]
        public void Register_GET_ReturnsView()
        {
            // Arrange
            var controller = new AccountController(null, null, null, null);

            // Act
            var result = controller.Register();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void LogIn_GET_ReturnsViewWithModel()
        {
            // Arrange
            var controller = new AccountController(null, null, null, null);

            // Act
            var result = controller.LogIn("test-url");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LogInVM>(viewResult.Model);
            Assert.Equal("test-url", model.ReturnUrl);
        }

        [Fact]
        public void AccessDenied_ReturnsView()
        {
            // Arrange
            var controller = new AccountController(null, null, null, null);

            // Act
            var result = controller.AccessDenied();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ForgotPassword_GET_ReturnsView()
        {
            // Arrange
            var controller = new AccountController(null, null, null, null);

            // Act
            var result = controller.ForgotPassword();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ForgotPasswordConfirmation_ReturnsView()
        {
            // Arrange
            var controller = new AccountController(null, null, null, null);

            // Act
            var result = controller.ForgotPasswordConfirmation();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ChangePassword_GET_ReturnsView()
        {
            // Arrange
            var controller = new AccountController(null, null, null, null);

            // Act
            var result = controller.ChangePassword();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ResetPassword_GET_WithValidTokenAndEmail_ReturnsView()
        {
            // Arrange
            var controller = new AccountController(null, null, null, null);

            // Act
            var result = controller.ResetPassword("test@test.com", "token123");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ResetStudentPasswordVM>(viewResult.Model);
            Assert.Equal("test@test.com", model.Email);
            Assert.Equal("token123", model.Token);
        }

        [Fact]
        public void ResetPassword_GET_WithNullEmail_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AccountController(null, null, null, null);

            // Act
            var result = controller.ResetPassword(null, "token123");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}