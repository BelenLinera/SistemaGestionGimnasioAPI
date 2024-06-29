using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult GetGymClassById(int idGymClass)
        {
            GymClass gymClass = _gymClassService.GetGymClassById(idGymClass);
            if (gymClass == null)
            {
                return NotFound();
            }
            return Ok(gymClass);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllGymClasses()
        {
            List<GymClass> gymClasses = _gymClassService.GetAllGymClasses();
            return Ok(gymClasses);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateGymClass([FromBody] GymClassDto gymClassDto)
        {
            if (gymClassDto == null)
            {
                return BadRequest("La solicitud no puede ser nula");
            }
            GymClass createdGymClass = _gymClassService.CreateGymClass(gymClassDto);
            await _gymClassService.SaveChangesAsync();
            //return CreatedAtRoute(nameof(GetGymClassById), new { idGymClass = gymClassDto.IdGymClass }, gymClassDto);

            return CreatedAtRoute(nameof(GetGymClassById), new { idGymClass = createdGymClass.IdGymClass }, gymClassDto);
        }

        [HttpPut]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> EditGymClass(GymClassDto gymClassEdited, int idGymClass)
        {
            if (gymClassEdited == null || idGymClass == null)
            {
                return BadRequest();
            }
            GymClass gymClassEdit = _gymClassService.EditGymClass(gymClassEdited, idGymClass);
            if (gymClassEdit == null)
            {
                return NotFound($"La clase con id '{idGymClass}' no se encontro. ");
            }
            await _gymClassService.SaveChangesAsync();
            return Ok(gymClassEdited);
        }
        [HttpPost("cancel")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CancelClass(int idGymClass, DateTime dateToCancel)
        {
            if (dateToCancel == default)
            {
                return BadRequest("La fecha no puede ser nula.");
            }

            await _gymClassService.CancelClassAsync(idGymClass, dateToCancel);
            await _gymClassService.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{idGymClass}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteGymClass(int idGymClass)
        {
            var gymClassToDelete = _gymClassService.GetGymClassById(idGymClass);
            if(gymClassToDelete == null)
            {
                return NotFound("Clase inexistente");
            }
            _gymClassService.DeleteGymClass((GymClass)gymClassToDelete);
            await _gymClassService.SaveChangesAsync();
            return NoContent();
        }

    }
}