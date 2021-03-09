using Microsoft.Extensions.Hosting;
using Nocturne.Auth.Configuration;

namespace Nocturne.Auth.Server
{
    public class Program
    {
        public static void Main(string[] args)
            => new AppHostBuilder<Startup>(args).Start();

        public static IHostBuilder CreateHostBuilder(string[] args)
            => new AppHostBuilder<Startup>(args).GetHostBuilder();
    }
}
