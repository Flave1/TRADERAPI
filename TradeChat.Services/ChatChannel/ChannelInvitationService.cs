using System;
using System.Linq;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Models.ViewModels;
using TradeChat.Services.Email;
using TradeChat.Services.Models;
using TradeChat.Services.Repository;

namespace TradeChat.Services.ChatChannel
{
    public class ChannelInvitationService : IChannelInvitationService
    {
        private readonly IEmailService emailService;
        private readonly IChannelMemberService channelMemberService;
        private readonly IDocumentRepository<ChannelDocument> channelRepo;
        private readonly IDocumentRepository<ChannelInvitationDocument> invitationRepo;

        public ChannelInvitationService(
            IEmailService emailService,
            IChannelMemberService channelMemberService,
            IDocumentRepository<ChannelDocument> channelRepo,
            IDocumentRepository<ChannelInvitationDocument> invitationRepo
        )
        {
            this.emailService = emailService;
            this.channelMemberService = channelMemberService;
            this.channelRepo = channelRepo;
            this.invitationRepo = invitationRepo;
        }

        public async Task InviteAsync(InviteMemberRequest request, UserClaimsInfo user)
        {
            var channel = await channelRepo.FindByIdAsync(request.ChannelId);
            if (channel == null)
            {
                return;
            }

            var code = Guid.NewGuid().ToString().Replace("-", "");
            //normally verify that user is authorized to do this action
            //check if the user already belongs to the channel (use MS graph to query by email)
            //check if there is an existing invitation for this email
            var invitations = await invitationRepo.FilterByAsync(x => x.InvitedUserEmail == request.Email && x.ChannelId == request.ChannelId);
            if (invitations != null && invitations.Count() > 0)
            {
                //verify this code actually works
                Array.ForEach<ChannelInvitationDocument>(invitations.ToArray(), (async x =>
                {
                    x.InvitationCode = code;
                    x.Expiry = DateTime.UtcNow.AddDays(3);
                    await invitationRepo.ReplaceOneAsync(x);
                }));
            }
            else
            {
                var invitation = new ChannelInvitationDocument
                {
                    ChannelId = request.ChannelId,
                    InvitedUserEmail = request.Email,
                    TriggerUserId = user.Id,
                    InvitationCode = code,
                    Expiry = DateTime.UtcNow.AddDays(3)
                };
                await invitationRepo.InsertOneAsync(invitation);
            }

            var codeIviteUrl = $"{request.OriginUrl}/invitation?code={code}";
            await emailService.SendMemberInvitation(request.Email, channel.DisplayName, codeIviteUrl);
        }

        public async Task RedeemInvitationAsync(string code, UserClaimsInfo user)
        {
            var invitation = await invitationRepo.FindOneAsync(x => x.InvitationCode == code);
            if (invitation == null)
            {
                // expired
                return;
            }

            //if (!String.Equals(invitation.InvitedUserEmail, user.Email, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    // expired
            //    return;
            //}

            if (invitation.Expiry != null && (DateTime.UtcNow - invitation.Expiry).Value.TotalDays > 3)
            {
                // expired
                return;
            }

            await channelMemberService.AddToChannel(invitation.ChannelId, user);
            await invitationRepo.DeleteByIdAsync(invitation.Id);
        }
    }
}
