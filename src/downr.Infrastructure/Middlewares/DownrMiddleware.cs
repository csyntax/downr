namespace downr.Infrastructure.Middlewares
{
    using System.Linq;
    using System.Text.Unicode;
    using System.IO.Compression;
    using System.Text.Encodings.Web;

    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.WebEncoders;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.AspNetCore.ResponseCompression;

    using Markdig;

    using downr.Common;
    using downr.Services;
    using downr.Services.Posts;
    using downr.Infrastructure.Extensions;

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

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = textEncoderSettings);

            //services.Configure<DownrOptions>(config.GetSection("Downr"));

            services.AddSingleton<DownrOptions>(s => s.GetRequiredService< IOptions<DownrOptions>>().Value);

            services.AddSingleton<IMarkdownContentLoader, MarkdownContentLoader>();
            services.AddSingleton<IYamlIndexer, YamlIndexer>();
            services.AddScoped<IPostService, PostService>();

            services.TryAddSingleton(s => s.GetInstance<MarkdownPipelineBuilder>().UseYamlFrontMatter().Build());

            services.AddOptions();
            services.AddMemoryCache();

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(mimeTypes);
            });

           

            return services;
        }

        public static IApplicationBuilder UseDownr(this IApplicationBuilder app)
        {
            /*if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = GlobalConstants.WebRootPath;
            }*/

            //var rewriteOptions = new RewriteOptions();

            //rewriteOptions.Add(new RedirectRequests());
            //rewriteOptions.Add(new RewriteRequests());

           /* if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                rewriteOptions.AddRedirectToWww();

                app.UseExceptionHandler("/Error");
            }*/

            var fileProvider = new PhysicalFileProvider(GlobalConstants.ContentPath);
            var contentProvider = new FileExtensionContentTypeProvider();

            contentProvider.Mappings.Remove(".md");
            contentProvider.Mappings.Remove(".js");
            contentProvider.Mappings.Remove(".css");

            var staticFileOptions = new StaticFileOptions
            {
                FileProvider = fileProvider,
                ContentTypeProvider = contentProvider,
                RequestPath = new PathString("/posts"),
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }
            };

            /*app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });-*/

            app.UseMiddleware<DownrContentMiddleware>();

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = new PathString("/static")
            });
            app.UseStaticFiles(staticFileOptions);
            //app.UseRewriter(rewriteOptions);
            app.UseResponseCompression();
           

            return app;
        }
    }
}
