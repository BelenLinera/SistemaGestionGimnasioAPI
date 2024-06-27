using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SistemaGestionGimnasioApi.Controllers;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace SistemaGestionGimnasioApi.Tests
{
    [TestClass]
    public class AuthenticateControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IEmailService> _emailServiceMock;
        private Mock<IConfiguration> _configMock;
        private AuthenticateController _authenticateController;

        [TestInitialize]
        public void SetUp()
        {
            _userServiceMock = new Mock<IUserService>();
            _emailServiceMock = new Mock<IEmailService>();
            _configMock = new Mock<IConfiguration>();

            _configMock.SetupGet(config => config["Authentication:SecretForKey"]).Returns("SecretKey12345");
            _configMock.SetupGet(config => config["Authentication:Issuer"]).Returns("Issuer");
            _configMock.SetupGet(config => config["Authentication:Audience"]).Returns("Audience");

            _authenticateController = new AuthenticateController(_userServiceMock.Object, _configMock.Object, _emailServiceMock.Object);
        }



        //[TestMethod]
        //public async Task Authenticate_ValidCredentials_ReturnsToken()
        //{
        //    var credentialDto = new CredentialsDto { Email = "belu@example.com", Password = "wyJQfeBvjpuW1Ealc/35jQ==;0Awq0Qr3ykvYO9uAsaZUtiu/ejOdUbJl5WK9XQQ+JAk=" };
        //    var validateUserResult = new BaseResponse { Result = true, Message = "Valid" };
        //    var user = new User { Email = "belu55@example.com", UserType = "Admin", Name = "belu", LastName = "linera" };

        //    _userServiceMock.Setup(service => service.ValidateUser(credentialDto.Email, credentialDto.Password))
        //                    .ReturnsAsync(validateUserResult);
        //    _userServiceMock.Setup(service => service.GetUserByEmail(credentialDto.Email))
        //                    .ReturnsAsync(user);


        //    var result = await _authenticateController.Authenticate(credentialDto);


        //    Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        //    var okResult = result as OkObjectResult;
        //    Assert.IsNotNull(okResult);
        //    Assert.IsInstanceOfType(okResult.Value, typeof(string));
        //    var token = okResult.Value as string;

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_configMock.Object["Authentication:SecretForKey"]);
        //    var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = _configMock.Object["Authentication:Issuer"],
        //        ValidAudience = _configMock.Object["Authentication:Audience"],
        //        IssuerSigningKey = new SymmetricSecurityKey(key)
        //    }, out SecurityToken validatedToken);

        //    var jwtToken = (JwtSecurityToken)validatedToken;
        //    Assert.AreEqual(user.Email, jwtToken.Claims.First(x => x.Type == "sub").Value);
        //}

        [TestMethod]
        public async Task Authenticate_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var credentialDto = new CredentialsDto { Email = "test@example.com", Password = "wrongPassword" };
            var validateUserResult = new BaseResponse { Result = false, Message = "Wrong Email or password" };

            _userServiceMock.Setup(service => service.ValidateUser(credentialDto.Email, credentialDto.Password))
                            .ReturnsAsync(validateUserResult);

            // Act
            var result = await _authenticateController.Authenticate(credentialDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Wrong Email or password", badRequestResult.Value);
        }

    }
}