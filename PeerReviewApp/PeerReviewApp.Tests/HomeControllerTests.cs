using Microsoft.AspNetCore.Mvc;
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
            var controller = new HomeController(null, null, null);

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
        }

        [Fact]
        public void RelTesting_ReturnsView()
        {
            // Arrange
            var controller = new HomeController(null, null, null);

            // Act
            var result = controller.RelTesting();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}