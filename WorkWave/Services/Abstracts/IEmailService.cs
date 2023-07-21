namespace WorkWave.Services.Abstracts
{
    public interface IEmailService
    {
            void SendEmailAsync(string email, string subject, string message);
        
    }
}
