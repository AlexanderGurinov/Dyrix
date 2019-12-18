using System;
using System.Threading.Tasks;
using Xunit;

namespace Gurinov.Microsoft.Cds.Tests
{
    public sealed class CdsClientTest : IClassFixture<CdsClientFixture>
    {
        private readonly ICdsClient _cdsClient;

        public CdsClientTest(CdsClientFixture cdsClientFixture) => 
            _cdsClient = cdsClientFixture?.CdsClient ?? throw new ArgumentNullException(nameof(CdsClient));

        [Fact]
        public async Task WhoAmI()
        {
            var (code, _, _) = await _cdsClient.GetAsync("WhoAmI()");
            Assert.Equal(200, code);
        }
    }
}
