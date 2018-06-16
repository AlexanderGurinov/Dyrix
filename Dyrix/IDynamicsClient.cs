using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Dyrix
{
    public interface IDynamicsClient
    {
        Task<JObject> GetAsync(string setName, Action<IQueryBuilder> configureDelegate);
        Task<(IReadOnlyDictionary<string, IEnumerable<string>>, JObject)> SendAsync(string method, string uri, IReadOnlyDictionary<string, IEnumerable<string>> headers = null, JObject content = null);
    }
}