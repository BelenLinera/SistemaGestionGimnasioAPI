using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class GymClassService : IGymClassService
    {
        private readonly SystemContext _context;
        private readonly IMapper _mapper;
        public GymClassService(SystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public GymClass GetGymClassById(int idGymClass)
        {
            try
            {
                return _context.GymClasses.Include(ta => ta.TrainerActivity).ThenInclude(t => t.Trainer).Include(ta => ta.TrainerActivity).ThenInclude(a => a.Activity).FirstOrDefault(c => c.IdGymClass == idGymClass);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la clase con Id {idGymClass}: {ex.Message}", ex);
            }
        }
        public List<GymClass> GetAllGymClasses()
        {
            return _context.GymClasses.Include(ta => ta.TrainerActivity).ThenInclude(t => t.Trainer).Include(ta => ta.TrainerActivity).ThenInclude(a => a.Activity).ToList();
        }
        public GymClass CreateGymClass(GymClassDto gymClassDto)
        {
            GymClass? gymClassEntity = _mapper.Map<GymClass>(gymClassDto);
            _context.GymClasses.Add(gymClassEntity);
            return gymClassEntity;
        }

        public GymClass EditGymClass( GymClassDto gymClass, int idGymClass)
        {
            GymClass gymClassToEdit = _context.GymClasses.SingleOrDefault(c => c.IdGymClass == idGymClass);
            if (gymClassToEdit == null)
            {
                return null;
            }
            GymClass gymClassEdited = _mapper.Map(gymClass, gymClassToEdit);
            _context.GymClasses.Update(gymClassEdited);
            return gymClassEdited;
        }

        public void DeleteGymClass(GymClass gymClassToDelete)
        {
            if (gymClassToDelete == null) 
            {
                throw new ArgumentNullException(nameof(gymClassToDelete));
            }
            _context.Remove(gymClassToDelete);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
