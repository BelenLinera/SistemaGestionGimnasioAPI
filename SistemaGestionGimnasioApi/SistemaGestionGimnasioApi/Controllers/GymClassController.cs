using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.Services.Implementations;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GymClassController : ControllerBase
    {
        private readonly IGymClassService _gymClassService;

        public GymClassController(IGymClassService gymClassService)
        {
            _gymClassService = gymClassService;
        }

        [HttpGet("{idGymClass}", Name = nameof(GetGymClassById))]
        //[Authorize]
        public IActionResult GetGymClassById(int idGymClass)
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
            GymClass gymClass = _gymClassService.GetGymClassById(idGymClass);
            if (gymClass == null)
            {
                return NotFound();
            }
            return Ok(gymClass);

            //}
            //return Forbid();
        }

        [HttpGet]
        //[Authorize]
        public IActionResult GetAllGymClasses()
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
            List<GymClass> gymClasses = _gymClassService.GetAllGymClasses();
            return Ok(gymClasses);
            //}
            //return Forbid(); 
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateGymClass([FromBody] GymClassDto gymClassDto)
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
            if (gymClassDto == null)
            {
                return BadRequest("La solicitud no puede ser nula");
            }
            GymClass createdGymClass = _gymClassService.CreateGymClass(gymClassDto);
            await _gymClassService.SaveChangesAsync();
            //return CreatedAtRoute(nameof(GetGymClassById), new { idGymClass = gymClassDto.IdGymClass }, gymClassDto);

            return CreatedAtRoute(nameof(GetGymClassById), new { idGymClass = createdGymClass.IdGymClass }, gymClassDto);

            //}
            //return Forbid();
        }

        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> EditGymClass(GymClassDto gymClassEdited, int idGymClass)
        {
            if (gymClassEdited == null || idGymClass == null)
            {
                return BadRequest();
            }
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
            GymClass gymClassEdit = _gymClassService.EditGymClass(gymClassEdited, idGymClass);
            if (gymClassEdit == null)
            {
                return NotFound($"La clase con id '{idGymClass}' no se encontro. ");
            }
            await _gymClassService.SaveChangesAsync();
            return Ok(gymClassEdited);
            //}
            //return Forbid();
        }

        [HttpDelete("{idGymClass}")]
        public async Task<IActionResult> DeleteGymClass(int idGymClass)
        {
            //string role = User.Claims.SingleOrDefault(c => c.Type.Contains("role")).Value;
            //if (role == "Admin")
            //{
            var gymClassToDelete = _gymClassService.GetGymClassById(idGymClass);
            if(gymClassToDelete == null)
            {
                return NotFound("Clase inexistente");
            }
            _gymClassService.DeleteGymClass((GymClass)gymClassToDelete);
            await _gymClassService.SaveChangesAsync();
            return NoContent();
            //}
            //return Forbid();
        }
    }
}