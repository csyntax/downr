namespace downr
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;

    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static async Task Main(string[] args) 
            => await CreateHostBuilder(args).RunConsoleAsync();

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.ConfigureAppConfiguration(appOptions =>
                     {
                         appOptions.SetBasePath(Directory.GetCurrentDirectory());
                         appOptions.AddJsonFile("appsettings.json", optional: false);
                         appOptions.AddCommandLine(args);
                         appOptions.AddEnvironmentVariables();
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

                         serverOptions.ListenAnyIP(Port);
                     })
                     .UseStartup<Startup>();
                 });

        private static int Port
        {
            get
            {
                try
                {
                    var port = Environment.GetEnvironmentVariable("PORT");

                    return int.Parse(port);
                }
                catch (ArgumentNullException)
                {
                    return 5000;
                }
            }
        }
    }
}