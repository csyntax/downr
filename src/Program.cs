namespace downr
{
    using System;
    using System.IO;
     using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;

    public class Program
    {   
        public static async Task Main(string[] args) => await CreateWebHostBuilder(args).Build().RunAsync();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
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

                    config.UseSystemd();
                    config.ListenAnyIP(5000);
                })
                .UseDefaultServiceProvider(config => config.ValidateScopes = true)
                .UseKestrel()
                .UseStartup<Startup>();
     }
}