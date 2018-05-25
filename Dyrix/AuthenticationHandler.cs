using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Dyrix
{
    internal sealed class AuthenticationHandler : DelegatingHandler
    {
        private readonly AuthenticationContext _context;
        private readonly string _resource;
        private readonly ClientCredential _credential;

        public AuthenticationHandler(AuthenticationContext context, string resource, ClientCredential credential, HttpMessageHandler innerHandler) : base(innerHandler)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }

            _resource = resource;
            _credential = credential ?? throw new ArgumentNullException(nameof(credential));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // It is a best practice to refresh the access token before every message request is sent. Doing so
            // avoids having to check the expiration date/time of the token. This operation is quick.
            var result = await _context.AcquireTokenAsync(_resource, _credential).ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
