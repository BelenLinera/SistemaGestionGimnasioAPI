using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.Services.Implementations;
using SistemaGestionGimnasioApi.Services.Interfaces;
using System.Security.Claims;

namespace SistemaGestionGimnasioApi.Controllers
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public AdminController(IAdminService adminService, IUserService userService)
        {
            _adminService = adminService;
            _userService = userService;
        }

        [HttpGet("{email}", Name = nameof(GetAdminByEmail))]
        public IActionResult GetAdminByEmail(string email)
        {
            try
            {
                User user = _adminService.GetAdminByEmail(email);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener el administrador por correo electrónico: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAllAdmins()
        {
            try
            {
                List<Admin> admins = _adminService.GetAllAdmins();
                return Ok(admins);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener la lista de administradores: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromBody] UserDto userDto)
        {
                if (userDto == null)
                {
                    return BadRequest("La solicitud no puede ser nula");
                }
                User userInUse = await _userService.GetUserByEmail(userDto.Email);
                if(userInUse != null)
                {
                return Conflict("Este email ya esta en uso");
                }
                Admin createdAdmin = _adminService.CreateAdmin(userDto);
                await _adminService.SaveChangesAsync();
            return CreatedAtRoute(nameof(GetAdminByEmail), new { email = userDto.Email }, userDto);
        }
        [HttpPut]
        public async Task<IActionResult> EditAdmin(EditUserDto adminEdited, string emailAdmin)
        {
            if (adminEdited == null || emailAdmin == null)
            {
                return BadRequest();
            }
                Admin adminEdit= _adminService.EditAdmin(adminEdited, emailAdmin);
                if(adminEdit == null)
                {
                    return NotFound($"El admin con correo electrónico '{emailAdmin}' no se encontró.");
            }
                await _adminService.SaveChangesAsync();
                return Ok(adminEdited);
        }
        [HttpDelete("{adminEmail}")]
        public async Task<IActionResult> DeleteUser(string adminEmail)
        {
                var adminToDelete = _adminService.GetAdminByEmail(adminEmail);
                if (adminToDelete == null)
                {
                    return NotFound("Admin inexistente");
                }
                _adminService.DeleteAdmin((Admin) adminToDelete);
                await _adminService.SaveChangesAsync();
                return NoContent();
        }
    }
}
