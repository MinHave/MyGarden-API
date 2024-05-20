using Microsoft.Extensions.Options;
using MyGarden_API.Models;

namespace MyGarden_API.Services
{
    public class MailService : IMailService
    {
        private readonly IMailSenderService _sender;
        private readonly SiteOptions _siteOptions;

        public MailService(IMailSenderService sender, IOptions<SiteOptions> siteOptions)
        {
            _sender = sender;
            _siteOptions = siteOptions.Value;
        }

        public async Task SendPasswordResetEmailAsync(string email, string username, string token)
        {
            string resetLink = $"{_siteOptions.FrontEndUrl}/resetPassword?token={System.Web.HttpUtility.UrlEncode(token)}";

            await _sender.SendEmailAsync(new EmailSendOptions()
            {
                TargetAddresses = new string[] { email },
                BodyIsHtml = true,
                Subject = "Reset password",
                Body = $@"Hey! {username}<br>
                Did you forget your password for MyGarden?<br>
                <br>
                Press the link below to change your password.<br>
                <a href=""{resetLink}"">Reset password</a><br>
                <br>
                If you didn't make the request, just ignore this message.
                <br>
                Thanks.<br>
                The team behind tavsogmatias.com",
            });
        }

        public async Task SendNewUserEmailAsync(string email, string username, string token)
        {
            string activateLink = $"{_siteOptions.FrontEndUrl}/activate?token={System.Web.HttpUtility.UrlEncode(token)}&email={System.Web.HttpUtility.UrlEncode(email)}";

            await _sender.SendEmailAsync(new EmailSendOptions()
            {
                TargetAddresses = new string[] { email },
                BodyIsHtml = true,
                Subject = "MyGarden account created",
                Body = $@"Hi {username},<br>
Your account at MyGarden is now created.<br>
<br>
Use the following link to activate your account.
<a href=""{activateLink}"">Activate account</a><br>
<br>
If the link isn't working, try to copy the link into your webbrowser.<br>
<br>
<br>
Kind regards,<br>
Team behind tavsogmatias.com",
            });

            await _sender.SendEmailAsync(new EmailSendOptions()
            {
                TargetAddresses = _siteOptions.AdminEmailAdresses,
                Subject = $"MyGarden: new user {email} created",
                BodyIsHtml = false,
                Body = "",
            });
        }

        public async Task SendAdminErrorEmailAsync(string title, string text, Exception exception)
        {
            string body;

            if (exception != null)
            {
                body = text + Environment.NewLine + Environment.NewLine + exception?.ToString();
            }
            else
            {
                body = text;
            }

            await _sender.SendEmailAsync(new EmailSendOptions()
            {
                TargetAddresses = _siteOptions.AdminEmailAdresses,
                Subject = $"MyGarden error: {title}",
                BodyIsHtml = false,
                Body = body,
            });
        }
    }
}
