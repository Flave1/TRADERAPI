using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeChat.Services.WebRequestHelper
{
    public interface IWebRequestService
    {
        Task GetAsync(string url, Dictionary<string, string> headers = null);

        Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null);
        Task<T> BasicGetAsync<T>(string url, Dictionary<string, string> headers = null);

        Task PostAsync<T>(string url, T data, Dictionary<string, string> headers = null);

        Task<T> PostAsync<T, Y>(string url, Y data, Dictionary<string, string> headers = null) where Y : class;
    }
}
