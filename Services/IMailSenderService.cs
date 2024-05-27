namespace MyGarden_API.Services
{
    public interface IMailSenderService
    {
        Task SendEmailAsync(EmailSendOptions request);
    }
}
