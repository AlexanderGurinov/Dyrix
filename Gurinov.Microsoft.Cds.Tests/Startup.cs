﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gurinov.Microsoft.Cds.Tests
{
    internal sealed class Startup
    {
        public static readonly IServiceProvider Provider = new ServiceCollection()
            .AddCdsClient(options =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddUserSecrets<Startup>()
                    .Build()
                    .GetSection(nameof(CdsClientOptions));

                options.ApiVersion = configuration[nameof(options.ApiVersion)];
                options.ClientId = configuration[nameof(options.ClientId)];
                options.ClientSecret = configuration[nameof(options.ClientSecret)];
                options.DirectoryId = configuration[nameof(options.DirectoryId)];
                options.Resource = configuration[nameof(options.Resource)];
            })
            .BuildServiceProvider();
    }
}