using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dyrix
{
    public sealed class DynamicsClient : IDynamicsClient
    {
        private readonly HttpClient _client;

        public DynamicsClient(HttpClient client) =>
            _client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> SendAsync(string method, string uri, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers = null, string content = null)
        {
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
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                }

                using (var response = await _client.SendAsync(request).ConfigureAwait(false))
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return ((int)response.StatusCode, response.Headers, responseContent);
                }
            }
        }

        public Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> SendAsync(string method, string uri, IEnumerable<KeyValuePair<string, string>> headers, string content = null) =>
            SendAsync(method, uri, headers.ToDictionary(i => i.Key, i => new[] { i.Value }.AsEnumerable()), content);

        public Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> SendAsync(string method, string uri, string header, string value, string content = null) =>
            SendAsync(method, uri, new[] { new KeyValuePair<string, IEnumerable<string>>(header, new[] { value }) }, content);

        public Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> SendAsync(string method, string uri, string content) =>
             SendAsync(method, uri, new Dictionary<string, IEnumerable<string>>(), content);

        public Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> GetAsync(string uri, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers = null, string content = null) =>
            SendAsync(nameof(HttpMethod.Get), uri, headers, content);

        public Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> GetAsync(string uri, IEnumerable<KeyValuePair<string, string>> headers, string content = null) =>
            SendAsync(nameof(HttpMethod.Get), uri, headers.ToDictionary(i => i.Key, i => new[] { i.Value }.AsEnumerable()), content);

        public Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> GetAsync(string uri, string header, string value, string content = null) =>
            SendAsync(nameof(HttpMethod.Get), uri, new[] { new KeyValuePair<string, IEnumerable<string>>(header, new[] { value }) }, content);
        
        public Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> GetAsync(string uri, string content) =>
            SendAsync(nameof(HttpMethod.Get), uri, new Dictionary<string, IEnumerable<string>>(), content);
    }
}
