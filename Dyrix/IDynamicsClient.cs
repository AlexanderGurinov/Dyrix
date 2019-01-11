using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dyrix
{
    public interface IDynamicsClient
    {
        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> SendAsync(string method, string uri, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers = null, string content = null);
        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> SendAsync(string method, string uri, IEnumerable<KeyValuePair<string, string>> headers, string content = null);
        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> SendAsync(string method, string uri, string header, string value, string content = null);
        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> SendAsync(string method, string uri, string content);

        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> GetAsync(string uri, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers = null, string content = null);
        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> GetAsync(string uri, IEnumerable<KeyValuePair<string, string>> headers, string content = null);
        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> GetAsync(string uri, string header, string value, string content = null);
        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> GetAsync(string uri, string content);

        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> PostAsync(string uri, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers = null, string content = null);
        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> PostAsync(string uri, IEnumerable<KeyValuePair<string, string>> headers, string content = null);
        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> PostAsync(string uri, string header, string value, string content = null);
        Task<(int, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, string)> PostAsync(string uri, string content);
    }
}