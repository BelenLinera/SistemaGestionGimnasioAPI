using AutoMapper;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly SystemContext _context;
        private readonly IMapper _mapper;
        public AdminService(SystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public User GetAdminByEmail(string email)
        {
            try
            {
                return _context.Admins.FirstOrDefault(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el administrador con Email {email}: {ex.Message}", ex);
            }
        }
        public List<Admin> GetAllAdmins()
        {
            return _context.Admins.Where(u => u.IsDeleted == false).ToList();
        }

        public Admin CreateAdmin(UserDto userDto)
        {
            Admin? userEntity = _mapper.Map<Admin>(userDto);
            _context.Admins.Add(userEntity);
            return userEntity;
        }
        public void EditAdmin(EditUserDto admin, string emailAdmin)
        {
            Admin adminToEdit = _context.Admins.SingleOrDefault(u => u.Email == emailAdmin);
            Admin adminEdited = _mapper.Map(admin, adminToEdit);

            _context.Admins.Update(adminEdited);

        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
