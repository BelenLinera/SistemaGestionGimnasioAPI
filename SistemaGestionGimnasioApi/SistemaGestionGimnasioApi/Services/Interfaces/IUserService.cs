using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IUserService
    {
        public BaseResponse ValidateUser(string email, string password);
        User GetUserByEmail(string email);
    }
}
