using static TestAuthApplication.Services.MailingService;

namespace TestAuthApplication.Services
{
    public interface IMailingService
    {
        public event MessageHandler? Notify;
        public Task<bool> SendEmailAsync(string? email, string subject, string message);
    }
}
