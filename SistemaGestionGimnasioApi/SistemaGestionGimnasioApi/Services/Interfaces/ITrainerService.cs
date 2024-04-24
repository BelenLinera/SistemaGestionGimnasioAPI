using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface ITrainerService
    {
        Trainer GetByEmail(string email);
        void UpdateByEmail(string email, TrainerDto updateDto);
        bool DeleteByEmail(string email);
    }
}
