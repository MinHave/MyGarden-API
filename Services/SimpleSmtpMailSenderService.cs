using Microsoft.Extensions.Options;
using MyGarden_API.Models;
using System.Net.Mail;
using System.Net;

namespace MyGarden_API.Services
{
    public class SimpleSmtpMailSenderService : IMailSenderService
    {
        private readonly SimpleSmtpOptions _options;

        public SimpleSmtpMailSenderService(IOptions<SimpleSmtpOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(EmailSendOptions request)
        {
            using SmtpClient client = new()
            {
                EnableSsl = _options.EnableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_options.UserName, _options.Password),
                Host = _options.Host,
                Port = _options.Port
            };

            var from = new MailAddress(request.FromEmail ?? _options.DefaultFromEmail, request.FromName ?? _options.DefaultFromName);

            var message = new MailMessage()
            {
                IsBodyHtml = request.BodyIsHtml,
                Body = request.Body,
                Subject = request.Subject,
                From = from,
                Sender = from
            };

            foreach (var address in request.TargetAddresses)
            {
                message.To.Add(new MailAddress(address));
            }

            if (request.Attachments != null)
            {
                foreach (var attachment in request.Attachments)
                {
                    message.Attachments.Add(new Attachment(new MemoryStream(attachment.Content), attachment.Name, attachment.Type));
                }
            }

            await client.SendMailAsync(message);
        }
    }
}
