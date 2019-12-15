using Microsoft.Extensions.DependencyInjection;

namespace Gurinov.Microsoft.Cds.Tests
{
    public sealed class DynamicsClientFixture
    {
        public ICdsClient DynamicsClient { get; } = Startup
            .Provider
            .GetRequiredService<ICdsClient>();
    }
}