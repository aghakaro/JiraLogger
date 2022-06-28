using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace JiraLogger
{
    public class JiraClient
    {
        private const string _defaultUrl = "https://perrknight.atlassian.net/";
        private string _userId = $"{Environment.UserName}@volo.global";
        private string _apiKey = "MDijmT31gRwmZBC0p5oXD6D2";

        private readonly bool _isEvenNum = DateTime.Now.Day % 2 == 0;

        private readonly HttpClient _httpClient;
        protected readonly JsonSerializerOptions JsonSerializerOptions;

        public JiraClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            SetDefaultClientHeaders(_httpClient);
            
            JsonSerializerOptions = new JsonSerializerOptions();
            JsonSerializerOptions.Converters.Add(new DateTimeConverterUsingDateTimeParse());
            JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            JsonSerializerOptions.AllowTrailingCommas = true;
            JsonSerializerOptions.WriteIndented = true;
        }  

        protected void SetDefaultClientHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected void SetRequestAuthHeaders(HttpRequestMessage request, string credentials)
        {
            request.Headers.Add("Authorization", "Basic " + credentials);
        }

        private string GetEncodedCredentials(string userId, string apiKey)
        {
            string credentials = $"{userId}:{apiKey}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
        }

        public async Task<object> PostAsync(string url, Body body, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();

            var requestContent = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GetEncodedCredentials(_userId, _apiKey));
            using var res = await _httpClient.PostAsync(_defaultUrl + url, requestContent, cancellationToken);
            _ = res.EnsureSuccessStatusCode();
            var content = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<object>(content, JsonSerializerOptions) ?? new();

        }

    }

    public class Body
    {
        public string timeSpent { get; set; }
        public string started { get; set; }
    }
}
