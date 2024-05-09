using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;
using SistemaGestionGimnasioApi.DBContext;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly SystemContext _context;
        private readonly IPaswordHasherService _paswordHasherService;
        public UserService(SystemContext context, IPaswordHasherService paswordHasherService)
        {
            _context = context;
            _paswordHasherService = paswordHasherService;
        }
        public BaseResponse ValidateUser(string email, string password)
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
        public User GetUserByEmail(string email)
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

    }
}
