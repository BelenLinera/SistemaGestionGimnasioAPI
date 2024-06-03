namespace SistemaGestionGimnasioApi.Services.Interfaces
{
    public interface IEmailService
    {
        Task<int> SendEmailAsync(string toEmail, string subject, string htmlContent);
    }
}
