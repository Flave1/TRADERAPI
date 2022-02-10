using System.Threading.Tasks;

namespace TradeChat.Services.Email
{
    public interface IEmailService
    {
        Task SendMemberInvitation(string email, string channelName, string code);
    }
}
