using CommonLibrary;
using Newtonsoft.Json;

namespace ApiHelper
{
    public class InnerApiClient : IInnerApiClient
    {
        private HttpClient _client;
        public InnerApiClient(string tokenEndpoint)
        {
            var httpClient = new HttpClient();
            var tokenProvider = new TokenProvider(httpClient, tokenEndpoint);
            var authenticatedHandler = new AuthenticatedHttpClientHandler(tokenProvider)
            {
                InnerHandler = new HttpClientHandler()
            };

            _client = new HttpClient(authenticatedHandler);
        }

        public async Task<BaseResponse<T>> GetAsync<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _client.SendAsync(request);
            var res = await GetResult<T>(response);
            return res;
        }

        public async Task<BaseResponse<T>> PostAsync<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var response = await _client.SendAsync(request);
            var res = await GetResult<T>(response);
            return res;
        }

        public async Task<BaseResponse<T>> PostAsync<T>(string url, HttpContent? content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            var response = await _client.SendAsync(request);
            var res = await GetResult<T>(response);
            return res;
        }

        public async Task<BaseResponse<T>> PutAsync<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            var response = await _client.SendAsync(request);
            var res = await GetResult<T>(response);
            return res;
        }

        public async Task<BaseResponse<T>> PutAsync<T>(string url, HttpContent? content)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = content;
            var response = await _client.SendAsync(request);
            var res = await GetResult<T>(response);
            return res;
        }

        public async Task<BaseResponse<T>> DeleteAsync<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await _client.SendAsync(request);
            var res = await GetResult<T>(response);
            return res;
        }

        private static async Task<BaseResponse<T>> GetResult<T>(HttpResponseMessage response)
        {
            string responseStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(responseStr))
                {
                    return new BaseResponse<T>()
                    {
                        IsSuccessStatusCode = response.IsSuccessStatusCode,
                        StatusCode = response.StatusCode,
                        Error = null,
                        Message = JsonConvert.DeserializeObject<T>(responseStr)
                    };
                }
            }
            return new BaseResponse<T>()
            {
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                StatusCode = response.StatusCode,
                Error = JsonConvert.DeserializeObject<ErrorResponse>(responseStr),
            };
        }
    }
}
