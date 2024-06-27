using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IReserveService
    {
        List<Reserve> GetAllReserves();
        List<Reserve> GetReservesByClient(string emailUser);
        Reserve GetReserveById(int id);
        Reserve CreateReserve(ReserveDto reserveDto, string emailClient);
        void ConfirmAssistance(Reserve reserve);

        void DeleteReserve(Reserve reserveToDelete);
        Task<bool> SaveChangesAsync();
    }
}
