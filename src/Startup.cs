namespace downr
{
    using downr.Middleware;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
            => services.AddDownr(this.config);

        public void Configure(IApplicationBuilder app)
            => app.UseDownr(this.config, this.env);
    }
}