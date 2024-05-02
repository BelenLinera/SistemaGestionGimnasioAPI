using AutoMapper;
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
            try
            {
                //creamos el nuevo objeto trainer con los datos que vienen del mapper
                Trainer? newTrainer = _mapper.Map<Trainer>(createTrainerDTO);

                //agregamos el nuevo trainer a la DB
                _context.Trainers.Add(newTrainer);
                _context.SaveChanges();

                return newTrainer;
            }
            catch (Exception)
            {
                throw new Exception("Error al crear un nuevo entrenador");
            }
        }

        public Trainer UpdateByEmail(string email, TrainerDto updateDto)
        {
            try
            {
                Trainer existingTrainer = _context.Trainers.FirstOrDefault(t => t.Email == email);

                if (existingTrainer == null)
                {
                    throw new NotFoundException($"No se encontró ningún entrenador con el correo electrónico: {email}");
                }

                // Actualizar propiedades del entrenador existente
                Trainer trainerEditing = _mapper.Map(updateDto, existingTrainer);

                _context.Trainers.Update(trainerEditing);
                _context.SaveChanges();
                return trainerEditing;
            }
            catch(Exception)
            {
                throw new Exception("Error al actualizar en entrenador");
            }
            
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

    }
}
