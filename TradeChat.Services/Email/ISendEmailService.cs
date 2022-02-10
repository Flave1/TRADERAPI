using System.Threading.Tasks;
using TradeChat.Services.Email.Models;

namespace TradeChat.Services.Email
{
    public interface ISendEmailService
    {
        Task SendEmailAsync(EmailMessage message);

        Task SendEmailWithTemplateAsync<T>(EmailMessage message, string templateId, T templateData);
    }
}
