using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        public TrainerController (ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        [HttpGet("{email}")]
        [Authorize(Policy = "Admin-Trainer")]
        public IActionResult GetByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) 
                {
                    return BadRequest("El correo electrónico no puede estar vacío.");
                }

                var trainer = _trainerService.GetByEmail(email);

                if (trainer == null)
                {
                    return NotFound($"No se encontró ningún entrenador con el correo electrónico: {email}");
                }

                return Ok(trainer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener el entrenador: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin-Trainer")]
        public IActionResult GetAllTrainers() 
        {
            List<Trainer> trainers = _trainerService.GetAllTrainers();
            return Ok(trainers);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateTrainer([FromBody] CreateTrainerDTO createTrainerDTO)
        {
           
            if (createTrainerDTO == null)
            {
                return BadRequest("La solicitud no puede ser nula");
            }
            if (_trainerService.GetByEmail(createTrainerDTO.Email) != null)
            {
                return Conflict("Este email ya está en uso");
            }

            Trainer createdTrainer = _trainerService.CreateTrainer(createTrainerDTO);
            if (createdTrainer == null) return NotFound("Alguna de las actividades no existe");
            await _trainerService.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByEmail), new { email = createTrainerDTO.Email }, createTrainerDTO);
            
        }

        [HttpPut("{email}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateByEmail( EditTrainerDto trainerupdated, string email)
        {
            
             if (string.IsNullOrEmpty(email))
             {
                 return BadRequest("El correo electrónico no puede estar vacío.");
             }

            Trainer trainerEdited = _trainerService.UpdateByEmail(email, trainerupdated);
            if (trainerEdited == null)  return NotFound($"El entrenador con correo electronico ´{email}´ no se encontró");
            if (trainerEdited.TrainerActivities == null) return NotFound("Alguna de las actividades no existe");
            await _trainerService.SaveChangesAsync();
            return Ok(trainerEdited); 

        }

        [HttpDelete("{email}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteByEmail(string email)
        {
            
                if (string.IsNullOrEmpty(email)) 
                {
                    return BadRequest("El correo electrónico no puede estar vacío.");
                }

                var success = _trainerService.DeleteByEmail(email);

                if (!success)
                {
                    return NotFound($"No se encontró ningún entrenador con el correo electrónico: {email}");
                }
                await _trainerService.SaveChangesAsync();
                return Ok(new {Message = "Baja logica realizada correctamente"});
            
            
        }
    }

}
