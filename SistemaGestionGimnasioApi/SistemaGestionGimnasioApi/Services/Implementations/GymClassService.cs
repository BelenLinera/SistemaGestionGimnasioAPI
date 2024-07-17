using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class GymClassService : IGymClassService
    {
        private readonly SystemContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public GymClassService(SystemContext context, IMapper mapper, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }
        public GymClass GetGymClassById(int idGymClass)
        {
            try
            {
                return _context.GymClasses.Include(ta => ta.TrainerActivity).ThenInclude(t => t.Trainer).Include(ta => ta.TrainerActivity).ThenInclude(a => a.Activity).FirstOrDefault(c => c.IdGymClass == idGymClass);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la clase con Id {idGymClass}: {ex.Message}", ex);
            }
        }
        public List<GymClass> GetAllGymClasses()
        {
            return _context.GymClasses
            .Include(gc => gc.TrainerActivity)
                .ThenInclude(ta => ta.Trainer)
            .Include(gc => gc.TrainerActivity)
                .ThenInclude(ta => ta.Activity)
            .Include(gc => gc.CancelledDates) 
            .ToList();
        }
        public GymClass CreateGymClass(GymClassDto gymClassDto)
        {
            GymClass? gymClassEntity = _mapper.Map<GymClass>(gymClassDto);
            _context.GymClasses.Add(gymClassEntity);
            return gymClassEntity;
        }

        public GymClass EditGymClass( GymClassDto gymClass, int idGymClass)
        {
            GymClass gymClassToEdit = _context.GymClasses.SingleOrDefault(c => c.IdGymClass == idGymClass);
            if (gymClassToEdit == null)
            {
                return null;
            }

            bool hasChanged = gymClassToEdit.Days != gymClass.Days || gymClassToEdit.TimeClass != gymClass.TimeClass;

            GymClass gymClassEdited = _mapper.Map(gymClass, gymClassToEdit);
            _context.GymClasses.Update(gymClassEdited);

            if (hasChanged)
            {
                NotifyReservesOfChanges(idGymClass, gymClassEdited, "La clase ha sido modificada", $"La clase fue pasada para el dia {gymClass.Days} a las {gymClass.TimeClass}.");
            }
            return gymClassEdited;
        }

        public async Task CancelClassAsync(int idGymClass, DateTime dateToCancel)
        {
            GymClass gymClass = await _context.GymClasses
                .Include(g => g.CancelledDates)
                .Include(gc => gc.TrainerActivity)
                .ThenInclude(ta => ta.Activity)
                .FirstOrDefaultAsync(c => c.IdGymClass == idGymClass);

            if (gymClass == null)
            {
                throw new InvalidOperationException("La clase no existe.");
            }

            CancelledClassDate cancelledDate = new CancelledClassDate
            {
                GymClassId = idGymClass,
                CancelledDate = dateToCancel
            };
            _context.CancelledClassDates.Add(cancelledDate);

            List<Reserve> reservesToNotify = await _context.Reserves
                .Where(r => r.IdGymClass == idGymClass && r.DateClass.Date == dateToCancel.Date)
                .ToListAsync();

            foreach (var reserve in reservesToNotify)
            {
                var subject = "Clase cancelada";
                var htmlContent = $"<p>La clase de {gymClass.TrainerActivity.Activity.ActivityName} programada para el {dateToCancel:dd/MM/yyyy} a las {gymClass.TimeClass} ha sido cancelada.</p>";
                await _emailService.SendEmailAsync(reserve.ClientEmail, subject, htmlContent);
            }

            
        }

        public async Task DeleteGymClass(GymClass gymClassToDelete)
        {
            if (gymClassToDelete == null)
            {
                throw new ArgumentNullException(nameof(gymClassToDelete));
            }

            _context.Remove(gymClassToDelete);

            await _context.SaveChangesAsync();

            NotifyReservesOfChanges(gymClassToDelete.IdGymClass, gymClassToDelete, "Clase cancelada", $"La clase de {gymClassToDelete.TrainerActivity.Activity.ActivityName} los {gymClassToDelete.DayName} a las {gymClassToDelete.TimeClass} ha sido cancelada y ya no estará disponible.");
        }

        private async Task NotifyReservesOfChanges(int idGymClass, GymClass gymClass, string subject, string message)
        {
            DateTime now = DateTime.Now;

            List<Reserve> reservesToNotify = await _context.Reserves
                .Where(r => r.IdGymClass == idGymClass && r.DateClass >= now)
                .ToListAsync();

            foreach (var reserve in reservesToNotify)
            {
                var htmlContent = $"<p>La clase programada para el {reserve.DateClass:dd/MM/yyyy} a las {gymClass.TimeClass} ha sido modificada.</p><p>{message}</p>";
                await _emailService.SendEmailAsync(reserve.ClientEmail, subject, htmlContent);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
