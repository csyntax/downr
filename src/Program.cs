namespace downr
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static async Task Main(string[] args)
            => await CreateHostBuilder(args).RunConsoleAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) 
            => Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.ConfigureAppConfiguration(appOptions =>
                     {
                         appOptions.SetBasePath(Directory.GetCurrentDirectory());
                         appOptions.AddJsonFile("appsettings.json", optional: false);
                         appOptions.AddCommandLine(args);
                     });

                     webBuilder.ConfigureLogging(loggerOptions =>
                     {
                         loggerOptions.ClearProviders();
                         loggerOptions.AddConsole();
                         loggerOptions.AddDebug();
                     });

                     webBuilder.ConfigureKestrel(serverOptions =>
                     {
                         serverOptions.Limits.MaxConcurrentConnections = 100;
                         serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
                         serverOptions.Limits.MaxRequestBodySize = 10 * 1024;

                         serverOptions.ListenAnyIP(5000);
                     })
                     .UseStartup<Startup>();
                 });

        /*private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false);
                    config.AddCommandLine(args);
                })
                .ConfigureLogging((hostingContext, config) =>
                {
                    config.ClearProviders();
                    config.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    config.AddConsole();
                    config.AddDebug();
                })
                .ConfigureKestrel((hostingContext, config) =>
                {
                    config.Limits.MaxConcurrentConnections = 100;
                    config.Limits.MaxConcurrentUpgradedConnections = 100;
                    config.Limits.MaxRequestBodySize = 10 * 1024;

                    config.ListenAnyIP(5000);
                })
                .UseKestrel()
                .UseStartup<Startup>();*/
     }
}