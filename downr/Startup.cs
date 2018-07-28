namespace downr
{
    using System;
    using System.IO;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using downr.Services;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(this.configuration);

            services.AddSingleton<IYamlIndexer, YamlIndexer>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IYamlIndexer yamlIndexer)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();

            var contentPath = Path.Combine(env.WebRootPath, "..", "Posts");

            yamlIndexer.IndexContentFiles(contentPath);
        }
    }
}
