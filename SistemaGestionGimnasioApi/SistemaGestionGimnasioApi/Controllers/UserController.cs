using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserService _userService;
        public IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }
        [HttpPost]
        public async Task<IActionResult> RecoverPassword([Required(ErrorMessage = "El email es obligatorio"), EmailAddress(ErrorMessage = "El email no tiene un formato válido")] string email)
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
        [HttpPatch]
        public  async Task<IActionResult> ChangePassword(ChangePasswordDto newPassword)
        {
            User userEdited=await  _userService.ChangePassword(newPassword.tokenRecover, newPassword.newPassword);
            if (userEdited == null) return BadRequest("Token invalido");
            _userService.SaveChangesAsync();

            return Ok($"Su contraseña fue cambiada con exito");
        }
    }
}
