using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.Services.Interfaces;
using System.Security.Claims;

namespace SistemaGestionGimnasioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IUserService _userService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("{email}", Name = nameof(GetClientByEmail))]
        [Authorize]
        public IActionResult GetClientByEmail(string email)
        {
            User client = _clientService.GetClientByEmail(email);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpGet]
        [Authorize(Policy = "Admin-Trainer")]
        public IActionResult GetAllClients()
        {
            List<Client> clients = _clientService.GetAllClients();
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("La solicitud no puede ser nula");
            }
            if (_clientService.GetClientByEmail(userDto.Email) != null)
            {
                return Conflict("Este email ya esta en uso");
            }
            Client createdClient = _clientService.CreateClient(userDto);
            await _clientService.SaveChangesAsync();
            return CreatedAtRoute(nameof(GetClientByEmail), new { email = userDto.Email }, userDto);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditClient(EditUserDto clientEdited, string emailClient)
        {
            string Clientemail = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();

            if (Clientemail == emailClient || role == "Admin")
            {
                if (clientEdited == null || emailClient == null)
                {
                    return BadRequest();
                }
                Client clientEdit = _clientService.EditClient(clientEdited, emailClient);
                if (clientEdit == null)
                {
                    return NotFound($"El cliente con correo electrónico '{emailClient}' no se encontró.");
                }
                await _clientService.SaveChangesAsync();
                return Ok(clientEdited);
            }

            return Unauthorized();
        }

        [HttpPatch("{emailClient}/state")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateClientActiveState([FromRoute]string emailClient, bool autorizationToReserve)
        {
            Client existingClient =  _clientService.GetClientByEmail(emailClient);
            if (existingClient == null)
            {
                return NotFound($"El cliente con correo electrónico '{emailClient}' no se encontró.");
            }

            existingClient.AutorizationToReserve = autorizationToReserve;

            await _clientService.SaveChangesAsync();
            return Ok(existingClient);
        }

        [HttpDelete("{clientEmail}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string clientEmail)
        {
            string Clientemail = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();

            if (Clientemail == clientEmail || role == "Admin")
            {

                var clientToDelete = _clientService.GetClientByEmail(clientEmail);
                if (clientToDelete == null)
                {
                    return NotFound("Cliente inexistente");
                }
                _clientService.DeleteClient((Client)clientToDelete);
                await _clientService.SaveChangesAsync();
                return NoContent();
            }

            return Unauthorized();
        }
    }
}