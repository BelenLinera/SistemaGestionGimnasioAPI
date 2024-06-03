using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class TrainerService : ITrainerService
    {
        private readonly SystemContext _context;
        private readonly IPasswordService _paswordHasherService;
        private readonly IMapper _mapper;

        public TrainerService (SystemContext context, IMapper mapper, IPasswordService paswordHasherService)
        {
            _context = context;
            _paswordHasherService = paswordHasherService;
            _mapper = mapper;
        }

        public Trainer GetByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) //usar try-catch
                {
                    throw new ArgumentException("El correo electrónico no puede estar vacío.");
                }

                return _context.Trainers.FirstOrDefault(t => t.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el entrenador con Email {email}: {ex.Message}", ex);
            }
        }

        public List<Trainer> GetAllTrainers() 
        {
            return _context.Trainers.Where(t => t.IsDeleted == false).ToList();
        }

        public Trainer CreateTrainer(CreateTrainerDTO createTrainerDTO)
        {
            string passwordHash = _paswordHasherService.Hash(createTrainerDTO.Password);
            createTrainerDTO.Password = passwordHash;
            Trainer? newTrainer = _mapper.Map<Trainer>(createTrainerDTO);
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
