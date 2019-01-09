using Microsoft.Extensions.DependencyInjection;

namespace Dyrix.Tests
{
    public sealed class DynamicsClientFixture
    {
        public IDynamicsClient DynamicsClient { get; } = Startup
            .Provider
            .GetRequiredService<IDynamicsClient>();
    }
}
