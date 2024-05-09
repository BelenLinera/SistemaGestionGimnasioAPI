using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.Services.Interfaces;
using System.Security.Claims;
using System.Text;

namespace SistemaGestionGimnasioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutheticateController : ControllerBase
    {
        public IUserService _userService;
        public IConfiguration _config;

        public AutheticateController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }
        [HttpPost]
        public IActionResult Authenticate([FromBody] CredentialsDto credentialDto)
        {
            BaseResponse validateUserResult = _userService.ValidateUser(credentialDto.Email, credentialDto.Password);
            if (validateUserResult.Message == "Wrong Email or password")
            {
                return BadRequest(validateUserResult.Message);
            } 
            if (validateUserResult.Result)
            {
                User user = _userService.GetUserByEmail(credentialDto.Email);
                //configura la clave de seguridad
                var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]));
                //configura el objeto para la firma del token
                var signature = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);
                //configura las claims
                var claimsForToken = new List<Claim>();
                claimsForToken.Add(new Claim("sub", user.Email));
                claimsForToken.Add(new Claim("role", user.UserType));
                claimsForToken.Add(new Claim("name", user.Name));
                claimsForToken.Add(new Claim("lastName", user.LastName));
                //crea el token con las configuraciones
                var jwtSecurityToken = new JwtSecurityToken(
                    _config["Authentication:Issuer"],
                    _config["Authentication:Audience"],
                    claimsForToken,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(1),
                    signature);
                //convierte el toekn en string
                string tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                return Ok(tokenToReturn);

            }
            return BadRequest();
        }
    }
}
