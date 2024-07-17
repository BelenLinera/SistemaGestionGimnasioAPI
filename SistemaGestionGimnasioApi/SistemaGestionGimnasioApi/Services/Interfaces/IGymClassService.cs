using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IGymClassService
    {
        public GymClass GetGymClassById(int idGymClass);
        List<GymClass> GetAllGymClasses();
        GymClass CreateGymClass(GymClassDto gymClassDto);
        GymClass EditGymClass(GymClassDto gymClassEdited, int idGymClass);
        Task CancelClassAsync(int idGymClass, DateTime dateToCancel);
        Task DeleteGymClass(GymClass gymClassToDelete);
        Task<bool> SaveChangesAsync();
    }
}
