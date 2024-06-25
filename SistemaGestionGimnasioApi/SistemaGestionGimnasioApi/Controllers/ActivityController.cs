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
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;


        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet("{activityName}", Name = nameof(GetActivityByName))]
        public IActionResult GetActivityByName(string activityName)
        {
            try
            {
                Activity activity = _activityService.GetActivityByName(activityName);
                
                if (activity == null)
                {
                    return NotFound();
                }
                return Ok(activity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener la actividad con nombre: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAllActivities()
        {
            try
            {
                List<Activity> activities = _activityService.GetAllActivities();
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener la lista de actividades: " + ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateActivity([FromBody] ActivityDto activityDto)
        {
            if (activityDto == null)
            {
                return BadRequest("La solicitud no puede ser nula");
            }
            if (_activityService.GetActivityByName(activityDto.ActivityName) != null)
            {
                return Conflict("Este nombre ya esta en uso");
            }
            Activity createdAactivity = _activityService.CreateActivity(activityDto);
            await _activityService.SaveChangesAsync();
            return CreatedAtRoute(nameof(GetActivityByName), new { activityName = activityDto.ActivityName }, activityDto);
        }

        [HttpPut]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> EditActivity(ActivityDto activityEdited, string activityName)
        {
            if (activityEdited == null || activityName == null)
            {
                return BadRequest();
            }
            Activity activityEdit = _activityService.EditActivity(activityEdited, activityName);
            if (activityEdit == null)
            {
                return NotFound($"La actividad con nombre '{activityName}' no se encontró.");
            }
            await _activityService.SaveChangesAsync();
            return Ok(activityEdited);
        }

        [HttpDelete("{activityName}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteActivity(string activityName)
        {
            var activityToDelete = _activityService.GetActivityByName(activityName);
            if (activityToDelete == null)
            {
                return NotFound("Actividad inexistente");
            }
            _activityService.DeleteActivity((Activity)activityToDelete);
            await _activityService.SaveChangesAsync();
            return NoContent();
        }
    }
}
