using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Dyrix
{
    public interface IDynamics : IDisposable
    {
        Task<JObject> GetStringAsync(string request);
    }
}