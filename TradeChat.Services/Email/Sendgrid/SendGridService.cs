using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using TradeChat.Services.Email.Models;

namespace TradeChat.Services.Email.Sendgrid
{
    public class SendGridService : ISendEmailService
    {
        private readonly SendGridClient client;

        public SendGridService(IOptions<SendGridOptions> options)
        {
            var config = options.Value;
            client = new SendGridClient(config.ApiKey);
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var msg = ComposeEmailAsync(message);
            await client.SendEmailAsync(msg);
        }

        public async Task SendEmailWithTemplateAsync<T>(EmailMessage message, string templateId, T templateData)
        {
            var msg = ComposeEmailAsync(message);
            msg.SetTemplateId(templateId);
            msg.SetTemplateData(templateData);
            await client.SendEmailAsync(msg);
        }

        private SendGridMessage ComposeEmailAsync(EmailMessage message)
        {
            var msg = new SendGridMessage
            {
                From = new EmailAddress(message.FromEmail, message.FromName),
                Subject = message.Subject,
                PlainTextContent = string.IsNullOrEmpty(message.Body) ? string.Empty : message.Body,
                HtmlContent = string.IsNullOrEmpty(message.Body) ? string.Empty :
                    string.IsNullOrEmpty(message.HtmlContent) ? message.Body : message.HtmlContent
            };

            msg.AddTo(new EmailAddress(message.ToEmail, message.ToName));
            return msg;
        }
    }
}
