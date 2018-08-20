namespace downr
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using downr.Middleware;

    public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Index", "/Posts");
                    options.Conventions.AddPageRoute("/Post", "/Posts/{slug}");
                    options.Conventions.AddPageRoute("/Category", "/Categories/{name}");
                });
            
            services.AddDownr(this.config);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc();
            app.UseDownr(this.config, env, loggerFactory);
        }
    }
}