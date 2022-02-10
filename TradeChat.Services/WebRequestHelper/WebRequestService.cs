using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TradeChat.Services.WebRequestHelper
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


        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            var retries = 0;
            var maxRetries = 3;

            do
            {
                try
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
                    if (response.IsSuccessStatusCode)
                    {
                        var stringData = await response.Content.ReadAsStringAsync();
                        var result = await response.Content.ReadFromJsonAsync<T>();
                        return result;
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var stringData = await response.Content.ReadAsStringAsync();
                        var result = await response.Content.ReadFromJsonAsync<T>();
                        return result;
                    }
                }
                catch (HttpRequestException)
                {
                    if (retries >= (maxRetries - 1))
                    {
                        //log error
                        throw;
                    }
                }

                retries++;
            }
            while (retries < maxRetries);

            throw new HttpRequestException("Failed to get data.");
        }

        public async Task<T> BasicGetAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            var retries = 0;
            var maxRetries = 3;

            do
            {
                try
                {
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            this.client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    var response = await this.client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var stringData = await response.Content.ReadAsStringAsync();
                        var result = await response.Content.ReadFromJsonAsync<T>();
                        return result;
                    }
                    else
                    {
                        var stringData = await response.Content.ReadAsStringAsync(); 
                    }
                }
                catch (HttpRequestException)
                {
                    if (retries >= (maxRetries - 1))
                    {
                        //log error
                        throw;
                    }
                }

                retries++;
            }
            while (retries < maxRetries);

            throw new HttpRequestException("Failed to get data.");
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

        public async Task<T> PostAsync<T, Y>(string url, Y data, Dictionary<string, string> headers = null) where Y : class
        {
            var retries = 0;
            var maxRetries = 3;

            do
            {
                try
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
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<T>();
                        return result;
                    }
                    else
                    {
                        var result = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (HttpRequestException)
                {
                    if (retries >= (maxRetries - 1))
                    {
                        //log error

                        throw;
                    }
                }

                retries++;

            } while (retries < maxRetries);

            throw new HttpRequestException("Failed to post data.");
        }
    }
}
