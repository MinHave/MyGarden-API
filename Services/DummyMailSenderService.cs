using Newtonsoft.Json;

namespace MyGarden_API.Services
{
    public class DummyMailSenderService : IMailSenderService
    {
        private readonly ILogger<DummyMailSenderService> _logger;

        public DummyMailSenderService(ILogger<DummyMailSenderService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(EmailSendOptions request)
        {
            var mailDirectory = Path.Combine(Environment.CurrentDirectory, "emails");

            Directory.CreateDirectory(mailDirectory);

            string filename = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {string.Join(" ", request.TargetAddresses)}.json";

            _logger.LogInformation("Would send email to {mails}: {subject}", string.Join(", ", request.TargetAddresses), request.Subject);

            File.WriteAllText(Path.Combine(mailDirectory, filename), JsonConvert.SerializeObject(request));

            return Task.CompletedTask;
        }
    }
}
