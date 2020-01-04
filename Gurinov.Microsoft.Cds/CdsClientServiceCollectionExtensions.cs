using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Gurinov.Microsoft.Cds
{
    public static class CdsClientServiceCollectionExtensions
    {
        public static IServiceCollection AddCdsClient(this IServiceCollection collection) => AddCdsClient(collection, options => { });
        
        public static IServiceCollection AddCdsClient(this IServiceCollection collection, Action<CdsClientOptions> configure)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            collection.Configure(configure)
                .AddTransient<AuthenticationHandler>()
                .AddHttpClient<ICdsClient, CdsClient>()
                .ConfigureHttpClient((provider, client) =>
                {
                    var options = provider.GetRequiredService<IOptions<CdsClientOptions>>().Value;

                    if (string.IsNullOrWhiteSpace(options.ApiVersion)) throw new ArgumentNullException(nameof(options.ApiVersion));
                    if (string.IsNullOrWhiteSpace(options.Resource)) throw new ArgumentNullException(nameof(options.Resource));

                    client.BaseAddress = new Uri($"{options.Resource}/api/data/v{options.ApiVersion}/");
                    client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                })
                .AddHttpMessageHandler<AuthenticationHandler>();

            return collection;
        }
    }
}