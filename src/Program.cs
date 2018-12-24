namespace downr
{
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static async Task Main(string[] args) => await BuildWebHost(args).RunAsync();

        private static IWebHost BuildWebHost(string[] args) =>
            /*  OLD
 *  WebHost
     .CreateDefaultBuilder(args)
     .UseStartup<Startup>()
     .UseKestrel(options => {
         options.Limits.MaxConcurrentConnections = 100;
         options.Limits.MaxConcurrentUpgradedConnections = 100;
         options.Limits.MaxRequestBodySize = 10 * 1024;
         options.Listen(IPAddress.Loopback, 5000);
     })
     .Build();*/

            WebHost
                .CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .UseStartup<Startup>()
                .Build();
     }
}