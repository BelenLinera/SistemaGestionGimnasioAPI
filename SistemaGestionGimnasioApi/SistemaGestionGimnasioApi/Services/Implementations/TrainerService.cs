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
            if (string.IsNullOrEmpty(email)) //usar try-catch
            {
                throw new ArgumentException("El correo electrónico no puede estar vacío.");
            }

            return _context.Trainers.FirstOrDefault(t => t.Email == email);
        }


        public void UpdateByEmail(string email, TrainerDto updateDto)
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

        public bool DeleteByEmail(string email)
        {
            var trainerToDelete = _context.Trainers.FirstOrDefault(t => t.Email == email);

            if (trainerToDelete == null)
            {
                return false; 
            }

            _context.Trainers.Remove(trainerToDelete);
            _context.SaveChanges();

            return true;
        }

    }
}
