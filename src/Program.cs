namespace downr
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        /* public static void Main(string[] args)
         {
             CreateWebHostBuilder(args).Build().Run();
         }*/

        public static async Task Main(string[] args) => await BuildWebHost(args).RunAsync();

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel(options =>
                {
                    options.Limits.MaxConcurrentConnections = 100;
                    options.Limits.MaxConcurrentUpgradedConnections = 100;
                    options.Limits.MaxRequestBodySize = 10 * 1024;
                })
                .UseStartup<Startup>()
                .Build();
    }
}