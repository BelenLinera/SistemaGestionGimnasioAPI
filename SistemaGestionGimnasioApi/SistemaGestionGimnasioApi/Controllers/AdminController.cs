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
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("{email}", Name = nameof(GetAdminByEmail))]
        //[Authorize]
        public IActionResult GetAdminByEmail(string email)
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
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

            //}
            //return Forbid();
        }

        [HttpGet]
        //[Authorize]
        public IActionResult GetAllAdmins()
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
            try
            {
                List<Admin> admins = _adminService.GetAllAdmins();
                return Ok(admins);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener la lista de administradores: " + ex.Message);
            }
            //}
            //return Forbid(); 
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateAdmin([FromBody] UserDto userDto)
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
                if (userDto == null)
                {
                    return BadRequest("La solicitud no puede ser nula");
                }
                if( _adminService.GetAdminByEmail(userDto.Email) != null  )
                {
                return Conflict("Este email ya esta en uso");
                }
                Admin createdAdmin = _adminService.CreateAdmin(userDto);
                await _adminService.SaveChangesAsync();
            return CreatedAtRoute(nameof(GetAdminByEmail), new { email = userDto.Email }, userDto);

            //}
            //return Forbid();
        }
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> EditAdmin(EditUserDto adminEdited, string emailAdmin)
        {
            if (adminEdited == null || emailAdmin == null)
            {
                return BadRequest();
            }
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
                Admin adminEdit= _adminService.EditAdmin(adminEdited, emailAdmin);
                if(adminEdit == null)
                {
                    return NotFound($"El admin con correo electrónico '{emailAdmin}' no se encontró.");
            }
                await _adminService.SaveChangesAsync();
                return Ok(adminEdited);
            //}
            //return Forbid();
        }
        [HttpDelete("{adminEmail}")]
        public async Task<IActionResult> DeleteUser(string adminEmail)
        {
            //string role = User.Claims.SingleOrDefault(c => c.Type.Contains("role")).Value;
            //if (role == "Admin")
            //{
                var adminToDelete = _adminService.GetAdminByEmail(adminEmail);
                if (adminToDelete == null)
                {
                    return NotFound("Admin inexistente");
                }
                _adminService.DeleteAdmin((Admin) adminToDelete);
                await _adminService.SaveChangesAsync();
                return NoContent();
            //}
            //return Forbid();
        }
    }
}
