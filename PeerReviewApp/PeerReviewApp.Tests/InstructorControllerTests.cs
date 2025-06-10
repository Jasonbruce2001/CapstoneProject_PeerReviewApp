using Microsoft.AspNetCore.Mvc;
using Moq;
using PeerReviewApp.Controllers;
using PeerReviewApp.Data;
using PeerReviewApp.Models;
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
    }
}