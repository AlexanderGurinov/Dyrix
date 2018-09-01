using System;
using System.Threading.Tasks;
using Xunit;

namespace Dyrix.Tests
{
    public sealed class DynamicsContextTest : IClassFixture<DynamicsContextFixture>
    {
        private readonly DynamicsContext _dynamicsContext;

        public DynamicsContextTest(DynamicsContextFixture fixture)
        {
            _dynamicsContext = fixture?.DynamicsContext ?? throw new ArgumentNullException(nameof(DynamicsContext));
        }

        [Fact]
        public async Task WhoAmI()
        {
            var r = await _dynamicsContext.CreateFunctionQuerySingle<WhoAmIResponse>(string.Empty, "WhoAmI", false)
                .GetValueAsync()
                .ConfigureAwait(false);

        }
    }
}
