using Microsoft.AspNetCore.Mvc.Infrastructure;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using AutoMapper;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class ActivityService : IActivityService
    {
        private readonly SystemContext _context;
        private readonly IMapper _mapper;
        public ActivityService(SystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Activity GetActivityByName(string activityName)
        {
            try
            {
                return _context.Activities.FirstOrDefault(a => a.ActivityName == activityName);
                //return _context.Activities.FirstOrDefault(a => a.ActivityName == activityName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la actividad con Nombre {activityName}: {ex.Message}", ex);
            }
        }
        public List<Activity> GetAllActivities()
        {
            return _context.Activities.ToList();   
        }

        public Activity CreateActivity(ActivityDto activityDto)
        {
            Activity? activityEntity = _mapper.Map<Activity>(activityDto);
            _context.Activities.Add(activityEntity);
            return activityEntity;
        }
        public Activity EditActivity(ActivityDto activity, string activityName)
        {
            Activity activityToEdit = _context.Activities.SingleOrDefault(u => u.ActivityName == activityName);
            if (activityToEdit == null)
            {
                return null;
            }
            Activity activityEdited = _mapper.Map(activity, activityToEdit);
            _context.Activities.Update(activityEdited);
            return activityEdited;
        }
        public void DeleteActivity(Activity activityToDelete)
        {
            if (activityToDelete == null)
            {
                throw new ArgumentNullException(nameof(activityToDelete));
            }
            _context.Remove(activityToDelete);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
