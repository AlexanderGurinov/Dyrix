using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Gurinov.Microsoft.Cds
{
    public static class CdsClientServiceCollectionExtensions
    {
        public static IServiceCollection AddDynamicsClient(this IServiceCollection collection, Action<CdsClientOptions> configure)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            var options = new CdsClientOptions();
            configure(options);
            return collection.AddDynamicsClient(options);
        }

        private static IServiceCollection AddDynamicsClient(this IServiceCollection collection, CdsClientOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(options.ApiVersion)) throw new ArgumentNullException(nameof(options.ApiVersion));
            if (string.IsNullOrWhiteSpace(options.ClientId)) throw new ArgumentNullException(nameof(options.ClientId));
            if (string.IsNullOrWhiteSpace(options.ClientSecret)) throw new ArgumentNullException(nameof(options.ClientSecret));
            if (string.IsNullOrWhiteSpace(options.DirectoryId)) throw new ArgumentNullException(nameof(options.DirectoryId));
            if (string.IsNullOrWhiteSpace(options.Resource)) throw new ArgumentNullException(nameof(options.Resource));

            collection.AddHttpClient<ICdsClient, CdsClient>(client =>
            {
                client.BaseAddress = new Uri($"{options.Resource}/api/data/v{options.ApiVersion}/");
                client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddHttpMessageHandler(provider =>
            {
                var context = new AuthenticationContext($"https://login.windows.net/{options.DirectoryId}/");
                var credential = new ClientCredential(options.ClientId, options.ClientSecret);
                return new AuthenticationHandler(context, options.Resource, credential);
            });

            return collection;
        }
    }
}
