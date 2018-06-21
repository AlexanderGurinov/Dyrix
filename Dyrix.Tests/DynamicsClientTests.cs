using System;
using Xunit;

namespace Dyrix.Tests
{
    public sealed class DynamicsClientTests : IClassFixture<DynamicsClientFixture>
    {
        private readonly IDynamicsClient _dynamicsClient;

        public DynamicsClientTests(DynamicsClientFixture dynamicsClientFixture)
        {
            _dynamicsClient = dynamicsClientFixture?.DynamicsClient ?? throw new ArgumentNullException(nameof(DynamicsClient));
        }

        [Fact]
        public void BasicQuery()
        {

        }
    }
}
