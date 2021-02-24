using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Calculator.WebApi.Services
{
    public class GoogleAuthToken
    {
        private readonly HttpClient _httpClient;
        
        public GoogleAuthToken(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAuthTokenAsync(string audience, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Add("Metadata-Flavor", "Google");
            var response = await _httpClient.GetAsync($"http://metadata.google.internal/computeMetadata/v1/instance/service-accounts/default/identity?audience={audience}", ct);
            
            return await response.Content.ReadAsStringAsync(ct);
        }
    }
}