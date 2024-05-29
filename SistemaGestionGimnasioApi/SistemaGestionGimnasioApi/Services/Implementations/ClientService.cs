using AutoMapper;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly SystemContext _context;
        private readonly IPasswordService _paswordHasherService;
        private readonly IMapper _mapper;
        public ClientService(SystemContext context, IMapper mapper, IPasswordService paswordHasherService)
        {
            _context = context;
            _mapper = mapper;
            _paswordHasherService = paswordHasherService;
        }
        public Client GetClientByEmail(string email)
        {
            try
            {
                return _context.Clients.FirstOrDefault(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el cliente con Email {email}: {ex.Message}", ex);
            }
        }
        public List<Client> GetAllClients()
        {
            return _context.Clients.Where(u => u.IsDeleted == false).ToList();
        }

        public Client CreateClient(UserDto userDto)
        {
            string passwordHash = _paswordHasherService.Hash(userDto.Password);
            userDto.Password = passwordHash;
            Client? userEntity = _mapper.Map<Client>(userDto);
            _context.Clients.Add(userEntity);
            return userEntity;
        }
        public Client EditClient(EditUserDto client, string emailClient)
        {
            Client clientToEdit = _context.Clients.SingleOrDefault(u => u.Email == emailClient);
            if (clientToEdit == null)
            {
                return null;
            }
            Client clientEdited = _mapper.Map(client, clientToEdit);
            _context.Clients.Update(clientEdited);
            return clientEdited;
        }
        public Client UpdateClientActiveState(string emailClient, bool autorizationToReserve)
        {
            Client clientToUpdate = _context.Clients.SingleOrDefault(u => u.Email == emailClient);

            if (clientToUpdate == null)
            {
                return null; 
            }

            clientToUpdate.AutorizationToReserve = autorizationToReserve;

            _context.Clients.Update(clientToUpdate);

            _context.SaveChanges();

            return clientToUpdate;
        }
        public void DeleteClient(Client clientToDelete)
        {
            if (clientToDelete == null)
            {
                throw new ArgumentNullException(nameof(clientToDelete));
            }
            _context.Remove(clientToDelete);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
