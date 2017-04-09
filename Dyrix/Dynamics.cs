using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Dyrix
{
    public sealed class Dynamics : IDynamics
    {
        private readonly HttpClient _httpClient;

        private Dynamics(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public static async Task<IDynamics> CreateAsync(string resource, string directoryId, string clientId, string clientSecret)
        {
            var authenticationContext = new AuthenticationContext($"https://login.windows.net/{directoryId}");
            var authenticationResult = await authenticationContext.AcquireTokenAsync(resource, new ClientCredential(clientId, clientSecret));

            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(2),
                BaseAddress = new Uri($"{resource}/api/data/v8.2/")
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);

            return new Dynamics(httpClient);
        }

        public async Task<JObject> GetStringAsync(string request)
        {
            return JsonConvert.DeserializeObject<JObject>(await _httpClient.GetStringAsync(request));
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
