using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse> ValidateUser(string email, string password);
        Task<User> GetUserByEmail(string email);

        Task<string> GeneratePasswordResetToken(User userToRecover);

        Task<string> ValidateToken(string token);
        Task<User> ChangePassword(string token, string newPassword);

        Task<bool> SaveChangesAsync();

    }
}
