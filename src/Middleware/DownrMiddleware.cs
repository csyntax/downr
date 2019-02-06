namespace downr.Middleware
{
    using System.Linq;
    using System.Text.Unicode;
    using System.IO.Compression;
    using System.Text.Encodings.Web;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.ResponseCompression;

    using Microsoft.Extensions.WebEncoders;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using downr.Services;
    using downr.Services.Posts;
    using downr.Middleware.Rules;

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
            
            services.AddOptions();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
        
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(mimeTypes);
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = textEncoderSettings);

            services.Configure<DownrOptions>(config.GetSection("Downr"));

            services.AddSingleton<IMarkdownContentLoader, MarkdownContentLoader>();
            services.AddSingleton<IYamlIndexer, YamlIndexer>();
            services.AddScoped<IPostService, PostService>();

            services
                .AddMvcCore()
                .AddAuthorization()
                .AddRazorPages(options => 
                {
                    options.Conventions.Clear();
                    options.Conventions.AddPageRoute("/Index", "/Posts");
                    options.Conventions.AddPageRoute("/Post", "/Posts/{slug}");
                    options.Conventions.AddPageRoute("/Category", "/Categories/{name}");
                });
                /*.AddViewOptions(options =>
                {
                    options.SuppressTempDataAttributePrefix = true;
                })
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.Clear();
                    options.Conventions.AddPageRoute("/Index", "/Posts");
                    options.Conventions.AddPageRoute("/Post", "/Posts/{slug}");
                    options.Conventions.AddPageRoute("/Category", "/Categories/{name}");
                });*/

            return services;
        }

        public static IApplicationBuilder UseDownr(this IApplicationBuilder app, 
            IConfiguration config, 
            IHostingEnvironment hostingEnvironment)
        {
            if (string.IsNullOrWhiteSpace(hostingEnvironment.WebRootPath))
            {
                hostingEnvironment.WebRootPath = Constants.WebRootPath;
            }

            if (hostingEnvironment.IsDevelopment())
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

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseMiddleware<DownrContentMiddleware>();

            app.UseStaticFiles();
            app.UseResponseCompression();
            app.UseStaticFiles(staticFileOptions);
            app.UseRewriter(rewriteOptions);
            app.UseMvc();

            return app;
        }
    }
}