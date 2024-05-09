namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IPaswordHasherService
    {
        string Hash(string password);
        bool Verify(string passwordHash, string inputPassword);
    }
}
