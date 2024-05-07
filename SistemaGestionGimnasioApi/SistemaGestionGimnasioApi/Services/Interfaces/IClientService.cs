using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IClientService
    {
        public User GetClientByEmail(string email);
        List<Client> GetAllClients();
        Client CreateClient(UserDto userDto);
        Client EditClient(EditUserDto clientEdited, string emailClient);

        void DeleteClient(Client clientToDelete);
        Task<bool> SaveChangesAsync();
    }
}
