using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeChat.Triggers.Services.WebRequestHelper
{
    public interface IWebRequestService
    {
        Task GetAsync(string url, Dictionary<string, string> headers = null);

        Task PostAsync<T>(string url, T data, Dictionary<string, string> headers = null);
    }
}
