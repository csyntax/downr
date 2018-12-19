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
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.AspNetCore.ResponseCompression;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.WebEncoders;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using downr.Rules;
    using downr.Services;
    using downr.Services.Posts;
    
    public static class DownrMiddleware
    {
        public static IServiceCollection AddDownr(this IServiceCollection services, IConfiguration config)
        {
            var mimeTypes = new string[]
            {
                "text/plain",
                "text/css",
                "text/html",
                "image/svg+xml",
                "application/javascript"
            };

            var textEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);

            services.AddMemoryCache();

            services.AddOptions();

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(mimeTypes);
            });            

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = textEncoderSettings;
            });

            services.Configure<DownrOptions>(config.GetSection("Downr"));

            services.AddSingleton(config);
            services.AddSingleton<IMarkdownContentLoader, MarkdownContentLoader>();
            services.AddSingleton<IYamlIndexer, YamlIndexer>();
            services.AddScoped<IPostService, PostService>();

            services
                .AddMvcCore()
                .AddAuthorization()
                .AddRazorPages(options =>
                {
                    options.Conventions.AddPageRoute("/Index", "/Posts");
                    options.Conventions.AddPageRoute("/Post", "/Posts/{slug}");
                    options.Conventions.AddPageRoute("/Category", "/Categories/{name}");
                });

            /*services
               .AddMvc()
               .AddRazorPagesOptions(options =>
               {
                   options.Conventions.AddPageRoute("/Index", "/Posts");
                   options.Conventions.AddPageRoute("/Post", "/Posts/{slug}");
                   options.Conventions.AddPageRoute("/Category", "/Categories/{name}");
               });*/

            return services;
        }

        public static IApplicationBuilder UseDownr(this IApplicationBuilder app,
            IConfiguration config,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(config.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = Constants.WebRootPath;
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            var provider = new FileExtensionContentTypeProvider();

            provider.Mappings.Remove(".md");
            provider.Mappings.Remove(".js");
            provider.Mappings.Remove(".css");

            var staticFileOptions = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Constants.ContentPath),
                RequestPath = "/posts",
                ContentTypeProvider = provider,
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }
            };

            var rewriteOptions = new RewriteOptions();

            rewriteOptions.Add(new RedirectRequests());
            rewriteOptions.Add(new RewriteRequests());

            app.UseStaticFiles();
            app.UseResponseCompression();
            app.UseStaticFiles(staticFileOptions);
            app.UseRewriter(rewriteOptions);
            app.UseMvc();

            app
                .ApplicationServices
                .GetRequiredService<IYamlIndexer>()
                .IndexContentFiles(Constants.ContentPath);

            return app;
        }
    }
}