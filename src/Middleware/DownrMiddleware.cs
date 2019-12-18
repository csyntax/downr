namespace downr.Middleware
{
    using System;
    using System.Linq;
    using System.Text.Unicode;
    using System.IO.Compression;
    using System.Text.Encodings.Web;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.WebEncoders;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.ResponseCompression;

    using downr.Services;
    using downr.Services.Posts;

    using Markdig;

    using downr.Middleware.Rules;
    using downr.Middleware.Extensions;

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
            services.AddHttpClient();

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

            services.TryAddSingleton(s => s.GetInstance<MarkdownPipelineBuilder>().UseYamlFrontMatter().Build());

            services.AddRazorPages(config =>
            {
                config.Conventions.Clear();
                config.Conventions.AddPageRoute("/Index", "/Posts");
                config.Conventions.AddPageRoute("/Post", "/Posts/{slug}");
                config.Conventions.AddPageRoute("/Category", "/Categories/{name}");
            });

            return services;
        }

        public static IApplicationBuilder UseDownr(this IApplicationBuilder app,
            IConfiguration config, IWebHostEnvironment env)
        {
            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = Constants.WebRootPath;
            }

            var rewriteOptions = new RewriteOptions();

            rewriteOptions.Add(new RedirectRequests());
            rewriteOptions.Add(new RewriteRequests());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                rewriteOptions.AddRedirectToWww();

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

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseMiddleware<DownrContentMiddleware>();

            app.UseStaticFiles();
            app.UseStaticFiles(staticFileOptions);
            app.UseRewriter(rewriteOptions);
            app.UseResponseCompression();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            return app;
        }
    }
}
