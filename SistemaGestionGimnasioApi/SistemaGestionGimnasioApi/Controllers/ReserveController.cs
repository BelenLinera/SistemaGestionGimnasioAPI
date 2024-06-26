﻿using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult GetReserveById(int id)
        {
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
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllReserves()
        {
            try
            {
                List<Reserve> reserves = _reserveService.GetAllReserves();
                return Ok(reserves);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener la lista de reservas: " + ex.Message);
            }
        }
        [HttpGet("my-reserves")]
        [Authorize(Policy = "Client")]
        public IActionResult GetReservesByClient()
        {
            try
            {
                string emailClient = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                List<Reserve> reserves = _reserveService.GetReservesByClient(emailClient);
                return Ok(reserves);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener la lista de reservas: " + ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> CreateReserve(ReserveDto reserveDto)
        {
            string emailClient = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            Client? client = _clientService.GetClientByEmail(emailClient);
            if (client.AutorizationToReserve == true)
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
        [Authorize(Policy = "Admin-Trainer")]
        public async Task<IActionResult> ConfirmAssistance(int id)
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
        [HttpDelete("{id}")]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> DeleteReserve(int id)
        {
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

            return Forbid("Usuario no autorizado");
        }

    }
}
