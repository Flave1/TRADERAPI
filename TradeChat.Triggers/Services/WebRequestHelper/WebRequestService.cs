using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TradeChat.Triggers.Services.WebRequestHelper
{
    public class WebRequestService : IWebRequestService
    {
        private readonly HttpClient client;
        public WebRequestService(IHttpClientFactory clientFactory)
        {
            client = clientFactory.CreateClient();
        }

        public async Task GetAsync(string url, Dictionary<string, string> headers = null)
        {
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    this.client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            var response = await this.client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to make request. Message: {result}");
            }
        }

        public async Task PostAsync<T>(string url, T data, Dictionary<string, string> headers = null)
        {
            this.client.DefaultRequestHeaders.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    this.client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var content = new StringContent(JsonSerializer.Serialize(data, serializeOptions), Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to post data. Message: {result}");
            }
        }
    }
}
