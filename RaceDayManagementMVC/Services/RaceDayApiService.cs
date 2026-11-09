using System.Net.Http.Json;

namespace RaceDayManagementMVC.Services
{
    public class RaceDayApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceDayApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("RaceDayApi");
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpRequestMessage CreateRequest(
            HttpMethod method, string endpoint)
        {
            var request = new HttpRequestMessage(method, endpoint);

            var cookie = _httpContextAccessor.HttpContext?
                .Session.GetString("ApiSessionCookie");

            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.TryAddWithoutValidation("Cookie", cookie);
            }

            return request;
        }

        public async Task<List<T>> GetAllAsync<T>(string endpoint)
        {
            using var request = CreateRequest(HttpMethod.Get, endpoint);
            using var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<T>>()
                   ?? new List<T>();
        }

        public async Task<T?> GetByIdAsync<T>(string endpoint, int id)
        {
            using var request = CreateRequest(
                HttpMethod.Get, $"{endpoint}/{id}");

            using var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<HttpResponseMessage> PostAsync<T>(
            string endpoint, T data)
        {
            using var request = CreateRequest(HttpMethod.Post, endpoint);
            request.Content = JsonContent.Create(data);

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(
            string endpoint, int id, T data)
        {
            using var request = CreateRequest(
                HttpMethod.Put, $"{endpoint}/{id}");

            request.Content = JsonContent.Create(data);

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteAsync(
            string endpoint, int id)
        {
            using var request = CreateRequest(
                HttpMethod.Delete, $"{endpoint}/{id}");

            return await _httpClient.SendAsync(request);
        }
    }
}