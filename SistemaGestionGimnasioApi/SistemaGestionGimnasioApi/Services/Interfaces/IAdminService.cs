using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IAdminService
    {
        public User GetAdminByEmail(string email);
        List<Admin> GetAllAdmins();
        Admin CreateAdmin(UserDto userDto);
        Admin EditAdmin(EditUserDto adminEdited, string emailClient);

        void DeleteAdmin(Admin adminToDelete);
        Task<bool> SaveChangesAsync();
    }
}
