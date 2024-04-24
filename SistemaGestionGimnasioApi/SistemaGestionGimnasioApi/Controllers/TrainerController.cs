﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
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

        [HttpPut("{email}")]
        public IActionResult UpdateByEmail(string email, [FromBody] TrainerDto updateDto)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("El correo electrónico no puede estar vacío.");
                }


                _trainerService.UpdateByEmail(email, updateDto);

                return Ok(true); // La actualización fue exitosa
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message); // No se encontró el entrenador
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al actualizar el entrenador: {ex.Message}");
            }
        }

        [HttpDelete("{email}")]
        public IActionResult DeleteByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) //validacion de email, tengo que agregarlo tmb en la entidad??
                {
                    return BadRequest("El correo electrónico no puede estar vacío.");
                }

                var success = _trainerService.DeleteByEmail(email);

                if (!success)
                {
                    return NotFound($"No se encontró ningún entrenador con el correo electrónico: {email}");
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar el entrenador: {ex.Message}");
            }
        }






    }

}
