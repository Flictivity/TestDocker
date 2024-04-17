using MimeKit;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace TestAuthApplication.Services
{
    public class MailingService : IMailingService
    {
        public delegate void MessageHandler(string message);
        public event MessageHandler? Notify;

        private static readonly SemaphoreSlim Semaphore = new(1, 1);
        public async Task<bool> SendEmailAsync(string? email, string subject, string message)
        {
            await Semaphore.WaitAsync();
            var sendMessage = new MimeMessage();
            sendMessage.From.Add(new MailboxAddress("Ветеринарная клиника", "computerservlse@mail.ru"));
            sendMessage.To.Add(MailboxAddress.Parse(email));
            sendMessage.Subject = subject;
            sendMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            SmtpClient client = new SmtpClient();

            try
            {
                await client.ConnectAsync("smpt.mail.ru", 465, true);
                await client.AuthenticateAsync("computerservlse@mail.ru", "sayetMTTeAJavYWFKkTJ");
                await client.SendAsync(sendMessage);
                Notify?.Invoke("Ok");
                return true;
            }
            catch (Exception ex)
            {
                Notify?.Invoke(ex.Message);
                return false;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
                Semaphore.Release();
            }
        }
    }
}
