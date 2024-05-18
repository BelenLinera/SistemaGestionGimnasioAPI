using SendGrid.Helpers.Mail;
using SendGrid;
using SistemaGestionGimnasioApi.Services.Interfaces;

namespace SistemaGestionGimnasioApi.Services.Implementations
{
    public class EmailService: IEmailService
    {
        private readonly string _sendGridApiKey;

        public EmailService(IConfiguration configuration)
        {
            _sendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            if (string.IsNullOrEmpty(_sendGridApiKey))
            {
                throw new InvalidOperationException("SendGrid API Key no configurada");
            }
        }
        public async Task<int> SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("trainingcenter333@gmail.com", "TrainingCenter");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);

            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                return (int)response.StatusCode;
            }

            return 200;
        }
    }
}
