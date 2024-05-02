using SendGrid.Helpers.Errors.Model;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class TrainerService: ITrainerService
    {
        private readonly SystemContext _context;

        public TrainerService (SystemContext context)
        {
            _context = context;
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
                //creamos el nuevo objeto trainer con los datos
                var newTrainer = new Trainer
                {
                    Name = createTrainerDTO.Name,
                    LastName = createTrainerDTO.LastName,
                    Email = createTrainerDTO.Email,
                    Password = createTrainerDTO.Password
                };

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

        public void UpdateByEmail(string email, TrainerDto updateDto)
        {
            try
            {
                var existingTrainer = _context.Trainers.FirstOrDefault(t => t.Email == email);

                if (existingTrainer == null)
                {
                    throw new NotFoundException($"No se encontró ningún entrenador con el correo electrónico: {email}");
                }

                // Actualizar propiedades del entrenador existente
                existingTrainer.Name = updateDto.Name;
                existingTrainer.LastName = updateDto.LastName;
                existingTrainer.Password = updateDto.Password;

                _context.Trainers.Update(existingTrainer);
                _context.SaveChanges();
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
