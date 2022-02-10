using System.Threading.Tasks;
using TradeChat.Services.Email.Models; 

namespace TradeChat.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly ISendEmailService emailService; 
        public EmailService(ISendEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task SendMemberInvitation(string email, string channelName, string code)
        {
            
            var message = new EmailMessage
            {
                FromEmail = "support@swiftcoretech.com",
                FromName = "SwiftCore Support",
                ToEmail = email,
                ToName = email,
                Subject = $"Invitation to Join {channelName} on Trade Chat App",
                Body = $"<p>Hello,</p><br/><p>You have been invited to {channelName}. Click <b><a href='{code}'>Here</a></b> to join the clannel</p>"
            };

            await emailService.SendEmailAsync(message);
        }
    }
}
