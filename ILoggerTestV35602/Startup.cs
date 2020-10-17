﻿using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

[assembly: FunctionsStartup(typeof(ILoggerTestV35602.Startup))]

namespace ILoggerTestV35602
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            try
            {
                ConfigureServices(builder.Services).BuildServiceProvider(true);

                throw new Exception("TEST EXCEPTION!!!");
            }
            catch (Exception ex)
            {
                var config = new TelemetryConfiguration(Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY"));
                var client = new TelemetryClient(config);
                client.TrackException(ex);
                client.Flush();

                throw;
            }



            //builder.Services.AddSingleton<IMyLoggerProvider, MyLoggerProvider>();
        }

        private IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMyLoggerProvider, MyLoggerProvider>();

            //services
            //    .AddLogging(loggingBuilder =>
            //        loggingBuilder.AddSerilog(dispose: true)
            //    )
            //    .AddTransient<IJsonPlaceholderClient, JsonPlaceholderClient>(client =>
            //        new JsonPlaceholderClient(Environment.GetEnvironmentVariable("BaseAddress"))
            //    )
            //    .AddTransient<IJsonPlaceholderService, JsonPlaceholderService>();

            return services;
        }
    }
}
