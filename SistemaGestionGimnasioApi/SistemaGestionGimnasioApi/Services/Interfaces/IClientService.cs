using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IClientService
    {
        public Client GetClientByEmail(string email);
        List<Client> GetAllClients();
        Client CreateClient(UserDto userDto);
        Client EditClient(EditUserDto clientEdited, string emailClient);
        Client UpdateClientActiveState(string emailClient, bool autorizationToReserve);

        void DeleteClient(Client clientToDelete);
        Task<bool> SaveChangesAsync();
    }
}
