using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;

namespace Dyrix
{
    public sealed class DynamicsClient : IDynamicsClient, IDisposable
    {
        private readonly ILogger<DynamicsClient> _logger;
        private readonly HttpClient _client;

        public DynamicsClient(IOptions<DynamicsClientOptions> options, ILogger<DynamicsClient> logger)
        {
            var optionsValue = options?.Value ?? throw new ArgumentNullException(nameof(options));

            var apiVersion = optionsValue.ApiVersion ?? throw new ArgumentNullException(nameof(optionsValue.ApiVersion));
            var clientId = optionsValue.ClientId ?? throw new ArgumentNullException(nameof(optionsValue.ClientId));
            var clientSecret = optionsValue.ClientSecret ?? throw new ArgumentNullException(nameof(optionsValue.ClientSecret));
            var directoryId = optionsValue.DirectoryId ?? throw new ArgumentNullException(nameof(optionsValue.DirectoryId));
            var resource = optionsValue.Resource ?? throw new ArgumentNullException(nameof(optionsValue.Resource));

            var context = new AuthenticationContext($"https://login.windows.net/{directoryId}");
            var credential = new ClientCredential(clientId, clientSecret);
            var innerHandler = new HttpClientHandler();
            var handler = new AuthenticationHandler(context, resource, credential, innerHandler);

            _client = new HttpClient(handler, true)
            {
                BaseAddress = new Uri($"{resource}/api/data/v{apiVersion}/"),
                DefaultRequestHeaders =
                {
                    // Every request should include the Accept header value of application/json
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
            // To ensure that there is no ambiguity about the OData version
            _client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            _client.DefaultRequestHeaders.Add("OData-Version", "4.0");

            _logger = logger;
        }

        //public Task Action(string name, string parameters) => PostAsync(name, parameters);

        private Task<(IReadOnlyDictionary<string, IEnumerable<string>>, JObject)> SendAsync(string method, string uri, JObject content) =>
             SendAsync(method, uri, new Dictionary<string, IEnumerable<string>>(), content);

        private Task<(IReadOnlyDictionary<string, IEnumerable<string>>, JObject)> SendAsync(string method, string uri, string header, string value, JObject content = null) =>
             SendAsync(method, uri, new Dictionary<string, string> { { header, value } }, content);

        private Task<(IReadOnlyDictionary<string, IEnumerable<string>>, JObject)> SendAsync(string method, string uri, IReadOnlyDictionary<string, string> headers, JObject content = null) =>
             SendAsync(method, uri, headers.ToDictionary(i => i.Key, i => new[] { i.Value }.AsEnumerable()), content);

        private async Task<(IReadOnlyDictionary<string, IEnumerable<string>>, JObject)> SendAsync(string method, string uri, IReadOnlyDictionary<string, IEnumerable<string>> headers = null, JObject content = null)
        {
            _logger?.LogTrace(uri);

            using (var request = new HttpRequestMessage(new HttpMethod(method), uri))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                if (content != null)
                {
                    request.Content = new StringContent(content.ToString(), Encoding.UTF8, "application/json");
                }

                using (var response = await _client.SendAsync(request).ConfigureAwait(false))
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var statusCode = (int)response.StatusCode;
                    if (304 < statusCode)
                    {
                        // Something went wrong
                        throw DynamicsClientException.Create(statusCode, responseContent);
                    }

                    var responseHeaders = response.Headers.ToDictionary(i => i.Key, i => i.Value);
                    return (responseHeaders, JObject.Parse(responseContent));
                }
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
