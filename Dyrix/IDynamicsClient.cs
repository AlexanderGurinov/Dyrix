using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Dyrix
{
    public interface IDynamicsClient
    {
        Task<JObject> QueryAsync(string entitySet, IEnumerable<string> attributes, QueryOptions options = null);
        Task<int> CountAsyc(string entitySet);
    }
}