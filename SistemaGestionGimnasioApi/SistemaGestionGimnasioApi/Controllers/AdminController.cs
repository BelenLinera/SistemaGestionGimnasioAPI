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
        private readonly IUserService _userService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("{email}")]
        public IActionResult GetAdminByEmail(string email)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                User user = _adminService.GetAdminByEmail(email);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);

            }
            return Forbid();
        }

        [HttpGet]
        public IActionResult GetAllAdmins()
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                List<Admin> admins = _adminService.GetAllAdmins();
            }
            return Forbid(); 
        }

        [HttpPost]
        public IActionResult CreateAdmin([FromBody] UserDto userDto)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            if (role == "Admin")
            {
                if (userDto == null)
                {
                    return BadRequest("La solicitud no puede ser nula");
                }
                Admin createdAdmin = _adminService.CreateAdmin(userDto);
                return CreatedAtAction("GetUserByEmail", new {email = createdAdmin.Email}, createdAdmin);
            }
            return Forbid();
        }
    }
}
