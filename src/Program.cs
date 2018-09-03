namespace downr
{
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static async Task Main(string[] args) => await BuildWebHost(args).RunAsync();

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
               /* .UseKestrel(options => {
                    options.Limits.MaxConcurrentConnections = 100;
                    options.Limits.MaxConcurrentUpgradedConnections = 100;
                    options.Limits.MaxRequestBodySize = 10 * 1024;

                    options.Listen(IPAddress.Loopback, 5000);
                })*/
                .Build();
    }
}