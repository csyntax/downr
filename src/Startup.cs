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
        private readonly IHostingEnvironment env;
        private readonly ILoggerFactory loggerFactory;

        public Startup(IConfiguration config, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory)
        {
            this.config = config;
            this.env = env;
            this.loggerFactory = loggerFactory;
        }

        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddDownr(this.config);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDownr(this.config, this.env, this.loggerFactory);
        }
    }
}