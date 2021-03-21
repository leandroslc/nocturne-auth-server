using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nocturne.Auth.Admin.Services.Initialization;
using Nocturne.Auth.Configuration;

namespace Nocturne.Auth.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateAppHostBuilder(args);

            if (HasInitializationArg(args))
            {
                var host = builder
                    .ConfigureDefaults(AddInitializationConfig)
                    .GetHostBuilder()
                    .Build();

                InitializationService.Run(host.Services);

                return;
            }

            builder.Start();
        }

        private static void AddInitializationConfig(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureAppConfiguration((hostContext, configBuilder) =>
            {
                configBuilder.AddJsonFile("initialize.json", optional: false);
            });

            webHostBuilder.ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
            });
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => CreateAppHostBuilder(args).GetHostBuilder();

        private static AppHostBuilder<Startup> CreateAppHostBuilder(string[] args) => new(args);

        private static bool HasInitializationArg(string[] args)
            => args.Any(arg => string.Equals(arg, "--init", StringComparison.InvariantCultureIgnoreCase));
    }
}
