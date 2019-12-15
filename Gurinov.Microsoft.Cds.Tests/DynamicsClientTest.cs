using System;
using System.Threading.Tasks;
using Xunit;

namespace Gurinov.Microsoft.Cds.Tests
{
    public sealed class DynamicsClientTest : IClassFixture<DynamicsClientFixture>
    {
        private readonly ICdsClient _dynamicsClient;

        public DynamicsClientTest(DynamicsClientFixture dynamicsClientFixture) => 
            _dynamicsClient = dynamicsClientFixture?.DynamicsClient ?? throw new ArgumentNullException(nameof(CdsClient));

        [Fact]
        public async Task WhoAmI()
        {
            var (code, _, _) = await _dynamicsClient.GetAsync("WhoAmI()");
            Assert.Equal(200, code);
        }
    }
}
