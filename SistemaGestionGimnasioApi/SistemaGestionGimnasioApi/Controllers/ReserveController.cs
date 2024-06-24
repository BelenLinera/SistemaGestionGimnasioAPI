using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace SistemaGestionGimnasioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _reserveService;
        private readonly IClientService _clientService;
        public ReserveController(IReserveService reserveService, IClientService clientService)
        {
            _reserveService = reserveService;
            _clientService = clientService;
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
                    return NotFound("Reserva no encontrada");

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
        [Authorize]
        public async Task<IActionResult> CreateReserve(ReserveDto reserveDto)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            string emailClient = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            Client? client = _clientService.GetClientByEmail(emailClient);
            if (role == "Client" && client.AutorizationToReserve == true)
            {
                if (reserveDto == null)
                {
                    return BadRequest("La solicitud no puede ser nula");
                }
                try
                {
                    Reserve createdReserve = _reserveService.CreateReserve(reserveDto, emailClient);
                    await _reserveService.SaveChangesAsync();
                    return CreatedAtRoute(nameof(GetReserveById), new { id = createdReserve.Id }, reserveDto);

                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Error al crear la reserva: " + ex.Message);
                }
            }
            else
            {

                return StatusCode(403, "Usuario no autorizado");
            }
        }
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> ConfirmAssistance(int id)
        {
            string role = User.Claims.SingleOrDefault(c => c.Type.Contains("role")).Value;
            if (role == "Trainer")
            {
                Reserve reserve = _reserveService.GetReserveById(id);
                if (reserve == null)
                {
                    return NotFound("Reserva inexistente");
                }
                _reserveService.ConfirmAssistance(reserve);
                await _reserveService.SaveChangesAsync();
                return Ok("Asistencia confirmada con exito");

            }
            return Forbid("Usuario no autorizado");
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReserve(int id)
        {
            //string role = User.Claims.SingleOrDefault(c => c.Type.Contains("role")).Value;
            //if (role == "Admin")
            //{
            Reserve? reserveToDelete = _reserveService.GetReserveById(id);
            string emailClient = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (reserveToDelete == null)
            {
                return NotFound("Reserva inexistente");
            }
            if (reserveToDelete.ClientEmail == emailClient)
            {
                _reserveService.DeleteReserve((Reserve)reserveToDelete);
                await _reserveService.SaveChangesAsync();
                return NoContent();
            }

            return Forbid();
        }

    }
}
