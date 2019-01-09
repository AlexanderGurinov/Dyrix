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
        public static IServiceCollection AddDynamicsClient(this IServiceCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            var configuration = collection.BuildServiceProvider()
                .GetRequiredService<IConfiguration>();

            collection.Configure<DynamicsClientOptions>(configuration.GetSection(nameof(DynamicsClientOptions)));

            var options = collection.BuildServiceProvider()
                .GetRequiredService<IOptions<DynamicsClientOptions>>().Value;

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

        //public static IServiceCollection AddDynamicsClient(this IServiceCollection serviceCollection, IConfiguration configuration = null)
        //{
        //    if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

        //    return serviceCollection.AddDynamicsClient((provider, options) =>
        //    {
        //        (configuration ?? provider.GetService<IConfiguration>())
        //            .Bind(nameof(DynamicsClientOptions), options);
        //    });
        //}

        //public static IServiceCollection AddDynamicsClient(this IServiceCollection collection, Action<DynamicsClientOptions> configureOptions)
        //{
        //    if (collection == null) throw new ArgumentNullException(nameof(collection));
        //    if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

        //    return collection.AddDynamicsClient((_, options) => configureOptions(options));
        //}

        //private static IServiceCollection AddDynamicsClient(this IServiceCollection collection, Action<IServiceProvider, DynamicsClientOptions> action) => collection
        //    .AddSingleton<IConfigureOptions<DynamicsClientOptions>>(
        //        provider => new ConfigureNamedOptions<DynamicsClientOptions>(Options.DefaultName, options => action(provider, options)))
        //    .AddSingleton<IDynamicsClient, DynamicsClient>();
    }
}
