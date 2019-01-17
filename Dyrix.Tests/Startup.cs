using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dyrix.Tests
{
    internal sealed class Startup
    {
        public static readonly IServiceProvider Provider = new ServiceCollection()
            .AddDynamicsClient(options =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddUserSecrets<Startup>()
                    .Build()
                    .GetSection(nameof(DynamicsClientOptions));

                options.ApiVersion = configuration[nameof(options.ApiVersion)];
                options.ClientId = configuration[nameof(options.ClientId)];
                options.ClientSecret = configuration[nameof(options.ClientSecret)];
                options.DirectoryId = configuration[nameof(options.DirectoryId)];
                options.Resource = configuration[nameof(options.Resource)];
            })
            .BuildServiceProvider();
    }
}
