using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dyrix
{
    public static class DyrixServiceCollectionExtensions
    {
        public static IServiceCollection AddDyrix(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return serviceCollection
                .Configure<DynamicsContextOptions>(configuration.GetSection(nameof(DynamicsContextOptions)))
                .AddSingleton<DynamicsContext>();
        }
    }
}
