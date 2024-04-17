namespace SistemaGestionGimnasioApi.Data.Entities
{
    public class Client : User
    {
        public bool AutorizationToReserve { get; set; } = false;
    }
}
