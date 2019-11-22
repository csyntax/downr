namespace downr
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

<<<<<<< HEAD
    using System.Text;

    using downr.Middleware;
=======
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
>>>>>>> small changes improve perfomance

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

<<<<<<< HEAD
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
=======
        public void Configure(IApplicationBuilder app)
            => app.UseDownr(this.env);
>>>>>>> small changes improve perfomance
    }
}