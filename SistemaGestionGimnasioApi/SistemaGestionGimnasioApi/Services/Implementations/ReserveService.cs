using AutoMapper;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class ReserveService : IReserveService
    {
        private readonly SystemContext _context;
        private readonly IMapper _mapper;
        public ReserveService(SystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Reserve GetReserveById(int id)
        {
            try
            {
                return _context.Reserves.FirstOrDefault(r => r.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la reserva con id {id}: {ex.Message}", ex);
            }
        }
        public List<Reserve> GetAllReserves()
        {
            return _context.Reserves.ToList();
        }
        public Reserve CreateReserve(ReserveDto reserveDto)
        {
            Reserve? reserveEntity = _mapper.Map<Reserve>(reserveDto);
            _context.Reserves.Add(reserveEntity);
            return reserveEntity;
        }
        public void DeleteReserve (Reserve reserveToDelete)
        {
            if (reserveToDelete == null)
            {
                throw new ArgumentException(nameof(reserveToDelete));
            }
            _context.Remove(reserveToDelete);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
