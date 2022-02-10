using System.Collections.Generic;

namespace TradeChat.Services.Zabo
{
    public interface IZaboAuthenticationService
    {
        Dictionary<string, string> Authenticate(string url);
    }
}
