using System;
using System.Collections.Generic;
using System.Text;

namespace TradeChat.Models.ViewModels
{
    public class ApikeyRequest
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public long Otp { get; set; }
    }
}
