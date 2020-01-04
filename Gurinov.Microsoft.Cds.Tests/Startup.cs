using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gurinov.Microsoft.Cds.Tests
{
    internal sealed class Startup
    {
        public static readonly IServiceProvider Provider = new ServiceCollection()
            .AddCdsClient(options =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddUserSecrets<Startup>()
                    .Build()
                    .GetSection(nameof(CdsClientOptions));

                configuration.Bind(options);
            })
            .BuildServiceProvider();
    }
}
