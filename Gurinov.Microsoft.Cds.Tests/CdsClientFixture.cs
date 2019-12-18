using Microsoft.Extensions.DependencyInjection;

namespace Gurinov.Microsoft.Cds.Tests
{
    public sealed class CdsClientFixture
    {
        public ICdsClient CdsClient { get; } = Startup
            .Provider
            .GetRequiredService<ICdsClient>();
    }
}