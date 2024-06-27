using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SistemaGestionGimnasioApi.Controllers;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Services.Interfaces;
using System.Collections.Generic;

namespace SistemaGestionGimnasioApi.Tests
{
    [TestClass]
    public class ActivityControllerTests
    {
        private Mock<IActivityService> _activityServiceMock;
        private ActivityController _activityController;

        [TestInitialize]
        public void SetUp()
        {
            _activityServiceMock = new Mock<IActivityService>();
            _activityController = new ActivityController(_activityServiceMock.Object);
        }

        [TestMethod]
        public void GetAllActivities_ReturnsOkWithListOfActivities()
        {
            // Arrange
            var expectedActivities = new List<Activity>
            {
                new Activity { IdActivity = 1, ActivityName = "Yoga" },
                new Activity { IdActivity = 2, ActivityName = "Pilates" }
            };

            _activityServiceMock.Setup(service => service.GetAllActivities()).Returns(expectedActivities);

            // Act
            var result = _activityController.GetAllActivities();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var activities = okResult.Value as List<Activity>;
            Assert.IsNotNull(activities);
            Assert.AreEqual(expectedActivities.Count, activities.Count);
        }


    }
}
