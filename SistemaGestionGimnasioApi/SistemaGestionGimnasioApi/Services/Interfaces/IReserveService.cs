using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IReserveService
    {
        Reserve GetReserveById(int id);
        List<Reserve> GetAllReserves();
        Reserve CreateReserve(ReserveDto reserveDto, string emailClient);
        void ConfirmAssistance(Reserve reserve);

        void DeleteReserve(Reserve reserveToDelete);
        Task<bool> SaveChangesAsync();
    }
}
