using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IReserveService
    {
        public Reserve GetReserveById(int id);
        List<Reserve> GetAllReserves();
        Reserve CreateReserve(ReserveDto reserveDto);

        void DeleteReserve(Reserve reserveToDelete);
        Task<bool> SaveChangesAsync();
    }
}
