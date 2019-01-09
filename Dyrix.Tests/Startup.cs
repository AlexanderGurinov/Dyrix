using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dyrix.Tests
{
    internal sealed class Startup
    {
        public static readonly IServiceProvider Provider = new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddUserSecrets<Startup>()
                .Build())
            .AddDynamicsClient()
            .BuildServiceProvider();
    }
}
