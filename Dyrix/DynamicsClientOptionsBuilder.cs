using System;

namespace Dyrix
{
    public sealed class DynamicsClientOptionsBuilder
    {
        public DynamicsClientOptionsBuilder(DynamicsClientOptions options) => 
            Options = options ?? throw new ArgumentNullException(nameof(options));

        public DynamicsClientOptions Options { get; }
    }
}