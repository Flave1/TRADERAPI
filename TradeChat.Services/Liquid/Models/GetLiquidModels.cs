using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace TradeChat.Services.Liquid.Models
{
    public class GetLiquidModels
    {
        [JsonProperty("models")]
        public List<object> Models { get; set; }
    }
}
