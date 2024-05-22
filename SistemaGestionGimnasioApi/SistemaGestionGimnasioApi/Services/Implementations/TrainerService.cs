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

        public Trainer CreateTrainer(CreateTrainerDTO createTrainerDTO)
        {

            List<int> acitivitiesId = createTrainerDTO.Activitys.Select(id => id).ToList();

            List<TrainerActivity> trainerActivities = new List<TrainerActivity>();
            foreach(var activity in acitivitiesId) 
            { 
                TrainerActivity trainerActivity = new TrainerActivity
                {
                    TrainerEmail = createTrainerDTO.Email,
                    IdActivity = activity
                };
                _context.TrainerActivities.Add(trainerActivity);
                trainerActivities.Add(trainerActivity);
            }

            Trainer? newTrainer = _mapper.Map<Trainer>(createTrainerDTO);
            newTrainer.TrainerActivities = trainerActivities;

            _context.Trainers.Add(newTrainer);

            return newTrainer;
            
        }

        public Trainer UpdateByEmail(string email, TrainerDto updateDto)
        {
            
            Trainer existingTrainer =  _context.Trainers.FirstOrDefault(t => t.Email == email);

            if (existingTrainer == null)
            {
                throw new NotFoundException($"No se encontró ningún entrenador con el correo electrónico: {email}");
            }

            // Actualizar propiedades del entrenador existente
            Trainer trainerEditing = _mapper.Map(updateDto, existingTrainer);

            _context.Trainers.Update(trainerEditing);
            return trainerEditing;
           
            
        }

        public bool DeleteByEmail(string email)
        {
            try
            {
                var trainerToDelete = _context.Trainers.FirstOrDefault(t => t.Email == email);

                if (trainerToDelete == null)
                {
                    return false;       //no se encuentra el entrenador entonces no hace cambios
                }

                //cambiamos el estado de la propiedad a true
                trainerToDelete.IsDeleted = true;

                _context.Trainers.Update(trainerToDelete);
                _context.SaveChanges();

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
