namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IPaswordService
    {
        string Hash(string password);
        bool Verify(string passwordHash, string inputPassword);


    }
}
