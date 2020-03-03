namespace downr
{
    using System.IO;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;


    using downr.Infrastructure.Middlewares;
    using downr.Common;
    using downr.Infrastructure;

    public class Startup
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            this.env = env;
            this.config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var infrastructureAssembly = typeof(DownrOptions).Assembly;

            services.AddSingleton(this.env);
            services.AddSingleton(this.config);

            services.AddRazorPages(config =>
            {
                config.Conventions.AddPageRoute("/Index", "/Posts");
                config.Conventions.AddPageRoute("/Post", "/Posts/{slug}");
                config.Conventions.AddPageRoute("/Category", "/Categories/{name}");
            })
            .AddApplicationPart(infrastructureAssembly);

            services.AddDownr(this.config);

            GlobalConstants.ContentPath = Path.Combine(Directory.GetCurrentDirectory(), "Posts");
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseDownr();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}