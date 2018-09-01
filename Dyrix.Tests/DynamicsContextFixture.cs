using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dyrix.Tests
{
    public sealed class DynamicsContextFixture
    {
        public DynamicsContext DynamicsContext { get; } = new ServiceCollection()
            .AddDyrix(new ConfigurationBuilder()
                .AddUserSecrets<DynamicsContextFixture>()
                .Build())
            .BuildServiceProvider()
            .GetRequiredService<DynamicsContext>();
    }
}
