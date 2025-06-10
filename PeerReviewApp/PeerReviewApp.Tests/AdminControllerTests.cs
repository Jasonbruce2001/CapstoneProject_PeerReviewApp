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
    }
}