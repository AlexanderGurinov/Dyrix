using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dyrix
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDynamicsClient(this IServiceCollection serviceCollection, IConfiguration configuration = null)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            
            return serviceCollection.AddDynamicsClient((provider, options) =>
            {
                (configuration ?? provider.GetService<IConfiguration>())
                    .Bind(nameof(DynamicsClientOptions), options);
            });
        }

        public static IServiceCollection AddDynamicsClient(this IServiceCollection collection, Action<DynamicsClientOptions> configureOptions)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

            return collection.AddDynamicsClient((_, options) => configureOptions(options));
        }

        private static IServiceCollection AddDynamicsClient(this IServiceCollection collection, Action<IServiceProvider, DynamicsClientOptions> action) => collection
            .AddSingleton<IConfigureOptions<DynamicsClientOptions>>(
                provider => new ConfigureNamedOptions<DynamicsClientOptions>(Options.DefaultName, options => action(provider, options)))
            .AddSingleton<IDynamicsClient, DynamicsClient>();
    }
}
