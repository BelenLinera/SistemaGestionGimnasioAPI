using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _reserveService;
        public ReserveController(IReserveService reserveService)
        {
            _reserveService = reserveService;
        }
        [HttpGet("{id}", Name = nameof(GetReserveById))]
        //[Authorize]
        public IActionResult GetReserveById(int id)
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
            try
            {
                Reserve reserve = _reserveService.GetReserveById(id);
                if (reserve == null)
                {
                    return NotFound();

                }
                return Ok(reserve);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener la reserva con id: " + ex.Message);
            }
            //}
            //return Forbid();
        }

        [HttpGet]
        //[Authorize]
        public IActionResult GetAllReserves()
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
            try
            {
                List<Reserve> reserves = _reserveService.GetAllReserves();
                return Ok(reserves);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener la lista de reservas: " + ex.Message);
            }
            //}
            //return Forbid(); 
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateReserve([FromBody] ReserveDto reserveDto)
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
            if (reserveDto == null)
            {
                return BadRequest("La solicitud no puede ser nula");
            }
            try
            {

                Reserve createdReserve = _reserveService.CreateReserve(reserveDto);
                await _reserveService.SaveChangesAsync();
                return CreatedAtRoute(nameof(GetReserveById), new { id = createdReserve.Id }, reserveDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al crear la reserva: " + ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserve(int id)
        {
            //string role = User.Claims.SingleOrDefault(c => c.Type.Contains("role")).Value;
            //if (role == "Admin")
            //{
            var reserveToDelete = _reserveService.GetReserveById(id);
            if (reserveToDelete == null)
            {
                return NotFound("Reserva inexistente");
            }
            _reserveService.DeleteReserve((Reserve)reserveToDelete);
            await _reserveService.SaveChangesAsync();
            return NoContent();
            //}
            //return Forbid();
        }

    }
}
