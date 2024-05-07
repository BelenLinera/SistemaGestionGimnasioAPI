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
        private readonly IMapper _mapper;
        public ClientService(SystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public User GetClientByEmail(string email)
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
