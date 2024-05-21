using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;
using System.Security.Policy;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly SystemContext _context;
        private readonly IPasswordService _paswordHasherService;
        public UserService(SystemContext context, IPasswordService paswordHasherService)
        {
            _context = context;
            _paswordHasherService = paswordHasherService;
        }
        public async Task<BaseResponse> ValidateUser(string email, string password)
        {
            BaseResponse response = new BaseResponse();
            User? userForLogin = _context.Users.SingleOrDefault(u => u.Email == email);
            if (userForLogin != null)
            {
                if (_paswordHasherService.Verify(userForLogin.Password, password))
                {
                    response.Result = true;
                    response.Message = "login succesfull";
                }
                else
                {
                    response.Result = false;
                    response.Message = "Wrong Email or passowrd";
                }
            }
            else
            {
                response.Result = false;
                response.Message = "Wrong Email or Passowrd";
            }
            return response;

        }
        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                return _context.Users.FirstOrDefault(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el administrador con Email {email}: {ex.Message}", ex);
            }
        }
        public async Task<string> GeneratePasswordResetToken(User UserToRecover)
        {
            string token = Guid.NewGuid().ToString().Substring(0, 8);
            UserToRecover.TokenRecover = token;
            _context.Users.Update(UserToRecover);
            return token;
        }
        public async Task<string?> ValidateToken(string token)
        {
            User user = _context.Users.FirstOrDefault(u => u.TokenRecover == token);
            return user?.TokenRecover;
        }
        public async Task<User> ChangePassword(string token, string newPassword) 
        {
            string passwordHash = _paswordHasherService.Hash(newPassword);
            User user = _context.Users.FirstOrDefault(u => u.TokenRecover == token);
            if (user == null) return null;
            user.Password = passwordHash;
            user.TokenRecover = null;
            return user;

        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

    }
}
