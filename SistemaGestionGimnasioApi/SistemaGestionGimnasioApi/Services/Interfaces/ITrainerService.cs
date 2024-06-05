using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface ITrainerService
    {
        Trainer GetByEmail(string email);
        List<Trainer> GetAllTrainers();
        Trainer CreateTrainer(CreateTrainerDTO createTrainerDTO);
        Trainer UpdateByEmail(string email, EditTrainerDto updateDto);
        bool DeleteByEmail(string email);
        Task<bool> SaveChangesAsync();
    }
}
