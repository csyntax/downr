﻿namespace downr
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.IO;
    using System.Threading.Tasks;

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


                     webBuilder.ConfigureLogging((hostingContext, loggerOptions) =>
                     {
                         var loggerConfig = hostingContext.Configuration.GetSection("Logging");

                         loggerOptions.ClearProviders();
                         loggerOptions.AddConfiguration(loggerConfig);
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
    }
}