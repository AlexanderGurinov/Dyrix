using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Dyrix
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDynamicsClient(this IServiceCollection collection, Action<DynamicsClientOptions> configure = default)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            var section = collection.BuildServiceProvider()
                .GetService<IConfiguration>()
                .GetSection(nameof(DynamicsClientOptions));

            collection.Configure<DynamicsClientOptions>(section);

            var options = collection.BuildServiceProvider()
                 .GetService<IOptions<DynamicsClientOptions>>()
                 .Value;

            configure?.Invoke(options);

            collection.AddHttpClient<IDynamicsClient, DynamicsClient>((provider, client) =>
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
