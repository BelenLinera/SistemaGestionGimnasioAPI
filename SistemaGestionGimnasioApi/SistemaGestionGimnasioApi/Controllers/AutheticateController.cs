﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.Services.Interfaces;
using System.Security.Claims;
using System.Text;
using SistemaGestionGimnasioApi.Services.Implementations;

namespace SistemaGestionGimnasioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutheticateController : ControllerBase
    {
        public IUserService _userService;
        public IEmailService _emailService;
        public IConfiguration _config;

        public AutheticateController(IUserService userService, IConfiguration config, IEmailService emailService)
        {
            _userService = userService;
            _config = config;
            _emailService = emailService;
        }
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] CredentialsDto credentialDto)
        {
            BaseResponse validateUserResult = await _userService.ValidateUser(credentialDto.Email, credentialDto.Password);
            if (validateUserResult.Message == "Wrong Email or password")
            {
                return BadRequest(validateUserResult.Message);
            }
            if (validateUserResult.Result)
            {
                User user = await _userService.GetUserByEmail(credentialDto.Email);
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

        [HttpPost("recoverPassword")]
        public async Task<IActionResult> RecoverPassword(string email)
        {
            try
            {
                User UserToRecover = await _userService.GetUserByEmail(email);
                if (UserToRecover == null) return NotFound("El usuario con ese Email no existe");

                string tokenToSend = await _userService.GeneratePasswordResetToken(UserToRecover);
                string htmlContent = $@"
            <html>
            <body>
                <h1>Recuperación de Contraseña</h1>
                <p>Hemos recibido una solicitud para restablecer tu contraseña. Utiliza el siguiente token para proceder:</p>
                <p>Token: {tokenToSend}</p>
                <p>Si no has solicitado esta acción, puedes ignorar este mensaje.</p>
            </body>
            </html>";

                int response = await _emailService.SendEmailAsync(email, "Recuperacion de contraseña", htmlContent);
                await _userService.SaveChangesAsync();
                if (response == 200) return Ok("Token enviado");
                return BadRequest($"Fallo la generacion del token{response}");
            }
            catch
            {
                return BadRequest();
            }
               
        }
    }
}
