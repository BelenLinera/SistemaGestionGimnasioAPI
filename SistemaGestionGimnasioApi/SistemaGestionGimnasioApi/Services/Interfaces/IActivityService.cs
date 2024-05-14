using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IActivityService
    {
        public Activity GetActivityByName(string activityName);
        List<Activity> GetAllActivities();
        Activity CreateActivity(ActivityDto activityDto);
        Activity EditActivity(ActivityDto activityEdited, string  activityName);
        void DeleteActivity(Activity activityToDelete);
        Task<bool> SaveChangesAsync();
    }
}
