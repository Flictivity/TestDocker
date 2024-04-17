using Microsoft.Extensions.Hosting;

namespace TestAuthApplication.Services
{
    public class MessagingService : BackgroundService
    {
        private readonly IMailingService _mailingService;
        private readonly IHostApplicationLifetime lifetime;

        public MessagingService(IMailingService mailingService, IHostApplicationLifetime lifetime)
        {
            _mailingService = mailingService;
            this.lifetime = lifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!await WaitForAppStartup(lifetime, stoppingToken))
                return;
            try
            {
                await Task.Delay(1000);
                var result = await _mailingService.SendEmailAsync("bulat_sa1@mail.ru","test","test1");
            }
            catch (Exception ex)
            {
                // обработка ошибки однократного неуспешного выполнения фоновой задачи
            }
        }

        static async Task<bool> WaitForAppStartup(IHostApplicationLifetime lifetime, CancellationToken stoppingToken)
        {
            // 👇 Создаём TaskCompletionSource для ApplicationStarted
            var startedSource = new TaskCompletionSource();
            using var reg1 = lifetime.ApplicationStarted.Register(() => startedSource.SetResult());

            // 👇 Создаём TaskCompletionSource для stoppingToken
            var cancelledSource = new TaskCompletionSource();
            using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

            // Ожидаем любое из событий запуска или запроса на остановку
            Task completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task).ConfigureAwait(false);

            // Если завершилась задача ApplicationStarted, возвращаем true, иначе false
            return completedTask == startedSource.Task;
        }
    }
}
