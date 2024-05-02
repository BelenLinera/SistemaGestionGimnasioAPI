using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface ITrainerService
    {
        Trainer GetByEmail(string email);
        List<Trainer> GetAllTrainers();
        Trainer CreateTrainer(CreateTrainerDTO createTrainerDTO);
        Trainer UpdateByEmail(string email, TrainerDto updateDto);
        bool DeleteByEmail(string email);
    }
}
