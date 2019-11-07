namespace downr
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    using System.Text;

    using downr.Middleware;

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
        {
            app.Use(async (contenxt, next) => 
            {
                string githubIndexRaw = $"https://raw.githubusercontent.com/YanaSlavcheva/OnlineCv/master/index.html";

                if (contenxt.Request.Path == "/cv")
                {
                    await contenxt.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes("CV"));
                }
 
                await next.Invoke();
            });

            app.UseDownr(this.config, this.env);
        }
    }
}