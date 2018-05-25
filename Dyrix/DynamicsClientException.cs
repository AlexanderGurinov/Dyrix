using System;
using Newtonsoft.Json.Linq;

namespace Dyrix
{
    public sealed class DynamicsClientException : Exception
    {
        internal static DynamicsClientException Create(int statusCode, string json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));

            var jObject = JObject.Parse(json);

            var error = jObject["error"];
            var code = (string)error?["code"];
            var message = (string)error?["message"] ?? string.Empty;

            var innerError = error?["innererror"];
            var innerMessage = (string)innerError?["message"] ?? string.Empty;
            var innerStackTrace = (string)innerError?["stacktrace"];

            var innerException = new DynamicsClientException(innerMessage)
            {
                _stackTrace = innerStackTrace
            };

            return new DynamicsClientException(message, innerException)
            {
                StatusCode = statusCode,
                Code = code
            };
        }

        private string _stackTrace;

        private DynamicsClientException(string message) : base(message)
        {
        }

        private DynamicsClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public int StatusCode { get; private set; }
        public string Code { get; private set; }
        public override string StackTrace => _stackTrace;
    }
}
