using System;
using System.Threading.Tasks;
using Xunit;

namespace Dyrix.Tests
{
    public sealed class DynamicsClientTest : IClassFixture<DynamicsClientFixture>
    {
        private readonly IDynamicsClient _dynamicsClient;

        public DynamicsClientTest(DynamicsClientFixture dynamicsClientFixture) => 
            _dynamicsClient = dynamicsClientFixture?.DynamicsClient ?? throw new ArgumentNullException(nameof(DynamicsClient));

        [Fact]
        public async Task WhoAmI()
        {
            var (code, _, _) = await _dynamicsClient.GetAsync("WhoAmI()");
            Assert.Equal(200, code);
        }
    }
}
