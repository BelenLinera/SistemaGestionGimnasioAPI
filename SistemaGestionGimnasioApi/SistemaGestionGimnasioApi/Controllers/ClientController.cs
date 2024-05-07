using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.Services.Interfaces;

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
        //[Authorize]
        public IActionResult GetClientByEmail(string email)
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Client")
            //{
            User user = _clientService.GetClientByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);

            //}
            //return Forbid();
        }

        [HttpGet]
        //[Authorize]
        public IActionResult GetAllClients()
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Client")
            //{
            List<Client> clients = _clientService.GetAllClients();
            return Ok(clients);
            //}
            //return Forbid(); 
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateClient([FromBody] UserDto userDto)
        {
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Client")
            //{
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

            //}
            //return Forbid();
        }
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> EditClient(EditUserDto clientEdited, string emailClient)
        {
            if (clientEdited == null || emailClient == null)
            {
                return BadRequest();
            }
            //string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToString();
            //if (role == "Admin")
            //{
            Client clientEdit = _clientService.EditClient(clientEdited, emailClient);
            if (clientEdit == null)
            {
                return NotFound($"El cliente con correo electrónico '{emailClient}' no se encontró.");
            }
            await _clientService.SaveChangesAsync();
            return Ok(clientEdited);
            //}
            //return Forbid();
        }
        [HttpDelete("{clientEmail}")]
        public async Task<IActionResult> DeleteUser(string clientEmail)
        {
            //string role = User.Claims.SingleOrDefault(c => c.Type.Contains("role")).Value;
            //if (role == "Client")
            //{
            var clientToDelete = _clientService.GetClientByEmail(clientEmail);
            if (clientToDelete == null)
            {
                return NotFound("Cliente inexistente");
            }
            _clientService.DeleteClient((Client)clientToDelete);
            await _clientService.SaveChangesAsync();
            return NoContent();
            //}
            //return Forbid();
        }
    }
}