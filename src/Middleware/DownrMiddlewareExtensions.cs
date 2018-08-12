namespace downr.Middleware
{
    using System.Linq;
    using System.IO.Compression;
    using System.Text.Unicode;
    using System.Text.Encodings.Web;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.AspNetCore.ResponseCompression;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.WebEncoders;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.DependencyInjection;

    using downr.Services;

    public static class DownrMiddlewareExtensions
    {
        public static void AddDownr(this IServiceCollection services)
        {
            string[] mimeTypes = new string[]
            {
                "text/plain",
                "text/css",
                "text/html",
                "image/svg+xml",
                "application/javascript"
            };

            var textEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(mimeTypes);
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = textEncoderSettings);

            services
               .AddMvc()
               .AddMvcLocalization()
               .AddRazorPagesOptions(options =>
               {
                   options.Conventions.AddPageRoute("/Index", "/Posts");
                   options.Conventions.AddPageRoute("/Post", "/Posts/{slug}");
                   options.Conventions.AddPageRoute("/Category", "/Categories/{name}");
               })
               .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.AddSingleton<IMarkdownContentLoader, MarkdownContentLoader>();
            services.AddSingleton<IYamlIndexer, YamlIndexer>();
        }

        public static void UseDownr(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = Constants.WebRootPath;
            }            

            var provider = new FileExtensionContentTypeProvider();

            provider.Mappings.Remove(".md");
            provider.Mappings.Remove(".js");
            provider.Mappings.Remove(".css");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseMvc();
            app.UseStaticFiles();
            app.UseResponseCompression();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Constants.ContentPath),
                RequestPath = "/posts",
                ContentTypeProvider = provider,
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }
            });

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope
                    .ServiceProvider
                    .GetRequiredService<IYamlIndexer>()
                    .IndexContentFiles(Constants.ContentPath);
            }
        }
    }
}