using System;
using Microsoft.Extensions.Configuration;

namespace Dyrix
{
    public static class DynamicsClientOptionsBuilderExtensions
    {
        public static DynamicsClientOptionsBuilder AddConfiguration(this DynamicsClientOptionsBuilder builder, IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration.Bind(builder.Options);
            return builder;
        }
    }
}
