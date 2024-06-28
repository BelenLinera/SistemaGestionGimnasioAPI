using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutoMapper;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Implementations;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using System.Linq;

namespace SistemaGestionGimnasioApi.Tests.Services
{
    [TestClass]
    public class ActivityServiceTests
    {
        private DbContextOptions<SystemContext> _options;
        private SystemContext _context;
        private Mock<IMapper> _mockMapper;
        private ActivityService _service;

        [TestInitialize]
        public void Initialize()
        {
            _options = new DbContextOptionsBuilder<SystemContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            _context = new SystemContext(_options);
            _mockMapper = new Mock<IMapper>();
            _service = new ActivityService(_context, _mockMapper.Object);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var activities = new[]
            {
                new Activity { IdActivity = 1, ActivityName = "Yoga", ActivityDescription = "Yoga classes" },
                new Activity { IdActivity = 2, ActivityName = "Pilates", ActivityDescription = "Pilates sessions" }
            };

            _context.Activities.AddRange(activities);
            _context.SaveChanges();
        }


        [TestMethod]
        public void CreateActivity_ShouldAddNewActivity()
        {
            // Arrange
            var activityDto = new ActivityDto { ActivityName = "Spinning", ActivityDescription = "Clase de Spinning" };
            var activity = new Activity { IdActivity = 3, ActivityName = "Spinning", ActivityDescription = "Clase de Spinning" };

            _mockMapper.Setup(m => m.Map<Activity>(It.IsAny<ActivityDto>())).Returns(activity);
            var dbSetMock = new Mock<DbSet<Activity>>();
            _context.Activities = dbSetMock.Object;

            // Act
            var result = _service.CreateActivity(activityDto);

            // Assert
            _mockMapper.Verify(m => m.Map<Activity>(activityDto), Times.Once);
            dbSetMock.Verify(db => db.Add(It.Is<Activity>(a => a.ActivityName == activityDto.ActivityName && a.ActivityDescription == activityDto.ActivityDescription)), Times.Once);
            Assert.AreEqual(activityDto.ActivityName, result.ActivityName);
            Assert.AreEqual(activityDto.ActivityDescription, result.ActivityDescription);
        }
    }
}
