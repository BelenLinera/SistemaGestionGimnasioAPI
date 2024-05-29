using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;
using System.Diagnostics;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class TrainerService : ITrainerService
    {
        private readonly SystemContext _context;
        private readonly IMapper _mapper;

        public TrainerService (SystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Trainer GetByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("El correo electrónico no puede estar vacío.");
                }

                return _context.Trainers
                    .Include(a => a.TrainerActivities)
                    .ThenInclude(ta => ta.Activity)
                    .FirstOrDefault(t => t.Email == email);
                    
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el entrenador con Email {email}: {ex.Message}", ex);
            }
        }

        public List<Trainer> GetAllTrainers() 
        {
            return _context.Trainers
                .Where(t => t.IsDeleted == false)
                .Include(a => a.TrainerActivities)
                .ThenInclude(ta => ta.Activity)
                .ToList();
        }

        public Trainer? CreateTrainer(CreateTrainerDTO createTrainerDTO)
        {
            List<int> activityIds = createTrainerDTO.Activities.Select(id => id).ToList();


            foreach (var activityId in activityIds)
            {
                if (!_context.Activities.Any(a => a.IdActivity == activityId))
                {
                    
                    return null;
                }
            }

            List<TrainerActivity> trainerActivities = new List<TrainerActivity>();
            foreach (var activityId in activityIds)
            {
                TrainerActivity trainerActivity = new TrainerActivity
                {
                    TrainerEmail = createTrainerDTO.Email,
                    IdActivity = activityId
                };
                _context.TrainerActivities.Add(trainerActivity);
                trainerActivities.Add(trainerActivity);
            }

            Trainer newTrainer = _mapper.Map<Trainer>(createTrainerDTO);
            newTrainer.TrainerActivities = trainerActivities;

            _context.Trainers.Add(newTrainer);

            return newTrainer;
        }


        public Trainer UpdateByEmail(string email, EditTrainerDto trainerDto)
        {
            Trainer existingTrainer = _context.Trainers.Include(t => t.TrainerActivities).FirstOrDefault(t => t.Email == email);
            foreach (var activityId in trainerDto.Activities)
            {
                if (!_context.Activities.Any(a => a.IdActivity == activityId))
                {
                    existingTrainer.TrainerActivities = null;
                    return  existingTrainer ;
                }
            }

            // Mantener las TrainerActivities existentes si sus Ids están presentes en el DTO de actualización
            var newActivityIds = trainerDto.Activities;
            existingTrainer.TrainerActivities.RemoveAll(ta => !newActivityIds.Contains(ta.IdActivity));

            // Agregar nuevas TrainerActivities que no estén en las existentes
            foreach (var activityId in newActivityIds)
            {
                if (!existingTrainer.TrainerActivities.Any(ta => ta.IdActivity == activityId))
                {
                    TrainerActivity trainerActivity = new TrainerActivity
                    {
                        TrainerEmail = email,
                        IdActivity = activityId
                    };
                    existingTrainer.TrainerActivities.Add(trainerActivity);
                }
            }

            // Actualizar el resto de los datos del entrenador
            Trainer trainerUpdated =_mapper.Map(trainerDto, existingTrainer);

            _context.Trainers.Update(existingTrainer);
            return existingTrainer;
        }
        public bool DeleteByEmail(string email)
        {
            try
            {
                var trainerToDelete = _context.Trainers.FirstOrDefault(t => t.Email == email);

                if (trainerToDelete == null)
                {
                    return false;
                }

                
                trainerToDelete.IsDeleted = true;
                _context.TrainerActivities.RemoveRange(_context.TrainerActivities.Where(ta => ta.TrainerEmail == trainerToDelete.Email));
                _context.Trainers.Update(trainerToDelete);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Error al eliminar el entrenador");
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

    }
}
