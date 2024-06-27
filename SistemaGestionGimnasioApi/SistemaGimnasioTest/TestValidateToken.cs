using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SistemaGestionGimnasioApi.Controllers;
using SistemaGestionGimnasioApi.Services.Interfaces;
using System.Threading.Tasks;

namespace UserControllerTests
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IEmailService> _emailServiceMock;
        private UserController _userController;

        [TestInitialize]
        public void SetUp()
        {
            _userServiceMock = new Mock<IUserService>();
            _emailServiceMock = new Mock<IEmailService>();
            _userController = new UserController(_userServiceMock.Object, _emailServiceMock.Object);
        }

        [TestMethod]
        public async Task ValidateToken_ValidToken_ReturnsOk()
        {
            string validToken = "validToken";
            _userServiceMock.Setup(s => s.ValidateToken(validToken)).ReturnsAsync("TokenValidated");

            var result = await _userController.ValidateToken(validToken);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual("TokenValidated", okResult.Value);
        }

    }
}
