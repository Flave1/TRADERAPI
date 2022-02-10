using System;
using System.Collections.Generic;
using System.Text;

namespace TradeChat.Models.ViewModels
{
    public class InviteMemberRequest
    {
        public string ChannelId { get; set; }
        public string Email { get; set; }
        public string OriginUrl { get; set; }
    }
}
