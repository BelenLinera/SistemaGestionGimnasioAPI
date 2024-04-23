using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly SystemContext _context;
        public AdminService(SystemContext context)
        {
            _context = context;
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
            return _context.Admins.Where(u => u.IsDeleted).ToList();
        }

        public Admin CreateAdmin(UserDto userDto)
        {
            var newAdmin = new Admin
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password,
                UserType = "Admin",
            };
            _context.Admins.Add(newAdmin);
            _context.SaveChanges();
            return newAdmin;
        }
    }
}
