namespace Sending_Email.Services
{
    public interface IMailingService
    {
        Task<string> SendEmailAsync(string email, string subject, string body);
    }
}
