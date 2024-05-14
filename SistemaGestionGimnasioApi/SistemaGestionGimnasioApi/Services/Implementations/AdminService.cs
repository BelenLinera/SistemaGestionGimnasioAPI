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
        private readonly IPaswordService _paswordHasherService;
        private readonly IMapper _mapper;
        public AdminService(SystemContext context, IMapper mapper, IPaswordService paswordHasherService)
        {
            _context = context;
            _mapper = mapper;
            _paswordHasherService = paswordHasherService;
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
            string passwordHash = _paswordHasherService.Hash(userDto.Password);
            userDto.Password = passwordHash;
            Admin? userEntity = _mapper.Map<Admin>(userDto);
            _context.Admins.Add(userEntity);
            return userEntity;
        }
        public Admin EditAdmin(EditUserDto admin, string emailAdmin)
        {
            Admin adminToEdit = _context.Admins.SingleOrDefault(u => u.Email == emailAdmin);
            if (adminToEdit == null)
            {
                return null;
            }
            Admin adminEdited = _mapper.Map(admin, adminToEdit);
            _context.Admins.Update(adminEdited);
            return adminEdited;
        }
        public void DeleteAdmin(Admin adminToDelete)
        {
            if (adminToDelete == null)
            {
                throw new ArgumentNullException(nameof(adminToDelete));
            }
            _context.Remove(adminToDelete);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
