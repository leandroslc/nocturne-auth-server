using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Nocturne.Auth.Configuration
{
    public class AppHostBuilder<TStartup>
        where TStartup : class
    {
        private readonly string[] args;
        private Action<IWebHostBuilder> configureWebHostDefaultsAction;
        private Action<IHostBuilder> configureHostAction;

        public AppHostBuilder(string[] args)
        {
            this.args = args;
        }

        public AppHostBuilder<TStartup> ConfigureDefaults(Action<IWebHostBuilder> configure)
        {
            configureWebHostDefaultsAction = configure;

            return this;
        }

        public AppHostBuilder<TStartup> Configure(Action<IHostBuilder> configure)
        {
            configureHostAction = configure;

            return this;
        }

        public IHostBuilder GetHostBuilder()
        {
            return CreateHostBuilder();
        }

        public void Start()
        {
            var host = CreateHostBuilder().Build();

            var configuration = (IConfiguration)host.Services.GetService(typeof(IConfiguration));

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Run(host);
        }

        private IHostBuilder CreateHostBuilder()
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options => options.AddServerHeader = false);
                    webBuilder.UseStartup<TStartup>();

                    configureWebHostDefaultsAction?.Invoke(webBuilder);
                })
                .UseSerilog((hostContext, loggerConfig) =>
                {
                    loggerConfig
                        .ReadFrom.Configuration(hostContext.Configuration);
                });

            configureHostAction?.Invoke(host);

            return host;
        }

        private static void Run(IHost host)
        {
            try
            {
                host.Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Host terminated unexpectedly");

                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
