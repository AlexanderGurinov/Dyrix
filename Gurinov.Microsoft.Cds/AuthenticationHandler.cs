using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Gurinov.Microsoft.Cds
{
    internal sealed class AuthenticationHandler : DelegatingHandler
    {
        private readonly AuthenticationContext _context;
        private readonly string _resource;
        private readonly ClientCredential _credential;

        public AuthenticationHandler(IOptions<CdsClientOptions> options)
        {
            var optionsValue = options?.Value ?? throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(optionsValue.ClientId)) throw new ArgumentNullException(nameof(optionsValue.ClientId));
            if (string.IsNullOrWhiteSpace(optionsValue.ClientSecret)) throw new ArgumentNullException(nameof(optionsValue.ClientSecret));
            if (string.IsNullOrWhiteSpace(optionsValue.DirectoryId)) throw new ArgumentNullException(nameof(optionsValue.DirectoryId));
            if (string.IsNullOrWhiteSpace(optionsValue.Resource)) throw new ArgumentNullException(nameof(optionsValue.Resource));

            _context = new AuthenticationContext($"https://login.windows.net/{optionsValue.DirectoryId}/");
            _resource = optionsValue.Resource;
            _credential = new ClientCredential(optionsValue.ClientId, optionsValue.ClientSecret);
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
