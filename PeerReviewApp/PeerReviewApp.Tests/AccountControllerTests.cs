using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PeerReviewApp.Controllers;
using PeerReviewApp.Data;
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

        [Fact]
        public async Task Register_POST_WithValidModel_RedirectsToHome()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<AppUser>>(
                mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(), null, null, null, null);
            var mockInstitutionRepo = new Mock<IInstitutionRepository>();
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            // Setup the institution repository to return empty list (student registration)
            mockInstitutionRepo.Setup(x => x.GetInstitutionsAsync()).ReturnsAsync(new List<Institution>());

            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockInstitutionRepo.Object, mockRoleManager.Object);

            var model = new RegisterVm { Username = "test", Email = "test@test.com", Password = "Test123!", ConfirmPassword = "Test123!" };

            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "Student"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await controller.Register(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);
        }

        [Fact]
        public async Task LogOut_RedirectsToHome()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<AppUser>>(
                mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(), null, null, null, null);

            var controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, null, null);

            // Act
            var result = await controller.LogOut();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);
        }

        [Fact]
        public void ValidateInstructorCode_WithValidCode_ReturnsTrue()
        {
            // Arrange
            var mockInstitutionRepo = new Mock<IInstitutionRepository>();
            var institutions = new List<Institution> { new Institution { Code = "ABC123" } };
            mockInstitutionRepo.Setup(x => x.GetInstitutionsAsync()).ReturnsAsync(institutions);

            var controller = new AccountController(null, null, mockInstitutionRepo.Object, null);

            // Act
            var result = controller.ValidateInstructorCode("ABC123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateInstructorCode_WithInvalidCode_ReturnsFalse()
        {
            // Arrange
            var mockInstitutionRepo = new Mock<IInstitutionRepository>();
            var institutions = new List<Institution> { new Institution { Code = "ABC123" } };
            mockInstitutionRepo.Setup(x => x.GetInstitutionsAsync()).ReturnsAsync(institutions);

            var controller = new AccountController(null, null, mockInstitutionRepo.Object, null);

            // Act
            var result = controller.ValidateInstructorCode("INVALID");

            // Assert
            Assert.False(result);
        }


    }
}