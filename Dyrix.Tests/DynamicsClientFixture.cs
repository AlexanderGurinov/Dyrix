using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dyrix.Tests
{
    public sealed class DynamicsClientFixture
    {
        public IDynamicsClient DynamicsClient { get; } = new ServiceCollection()
            .AddDynamicsClient(new ConfigurationBuilder().Build())
            .BuildServiceProvider()
            .GetService<IDynamicsClient>();
    }
}
