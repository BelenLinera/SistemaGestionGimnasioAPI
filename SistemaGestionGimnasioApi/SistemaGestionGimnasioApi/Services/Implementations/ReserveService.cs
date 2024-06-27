using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        public List<Reserve> GetReservesByClient(string emailUser)
        {
            return  _context.Reserves
                                .Include(r => r.GymClass)
                                    .ThenInclude(gc => gc.TrainerActivity)
                                        .ThenInclude(ta => ta.Activity)
                                .Include(r => r.GymClass)
                                    .ThenInclude(gc => gc.TrainerActivity)
                                        .ThenInclude(ta => ta.Trainer) 
                                .Where(r => r.ClientEmail == emailUser)
                                .OrderByDescending(r => r.DateClass)
                                .ToList();

        }

        public Reserve CreateReserve(ReserveDto reserveDto, string emailClient)
        {
            Reserve? reserveEntity = _mapper.Map<Reserve>(reserveDto);
            reserveEntity.ClientEmail = emailClient;
            _context.Reserves.Add(reserveEntity);
            return reserveEntity;
        }
        public void ConfirmAssistance(Reserve reserve)
        {
            reserve.ClientAttended = !reserve.ClientAttended;
            _context.Reserves.Update(reserve);
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
