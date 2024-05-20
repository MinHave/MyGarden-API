namespace MyGarden_API.Services
{
    public interface IMailService
    {
        Task SendAdminErrorEmailAsync(string title, string text, Exception exception);

        Task SendNewUserEmailAsync(string email, string username, string token);

        Task SendPasswordResetEmailAsync(string email, string username, string token);
    }
}
