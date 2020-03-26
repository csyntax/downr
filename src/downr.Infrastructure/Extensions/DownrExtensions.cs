namespace downr.Infrastructure.Extensions
{
    using System.IO;
    using System.Text.Unicode;
    using System.IO.Compression;
    using System.Text.Encodings.Web;

    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.WebEncoders;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.AspNetCore.ResponseCompression;

    using Markdig;

    using downr.Common;
    using downr.Services;
    using downr.Services.Posts;

    using downr.Infrastructure.Middlewares;

    public static class DownrExtensions
    {
        public static IServiceCollection AddDownrConfigurations(this IServiceCollection services, IConfiguration configuarion)
        {
            var textEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = textEncoderSettings);

            services.Configure<DownrOptions>(configuarion.GetSection("Downr"));

            return services;
        }

        public static IServiceCollection AddDownrServices(this IServiceCollection services)
        {
            services.TryAddSingleton(s => new MarkdownPipelineBuilder().UseYamlFrontMatter().Build());

            services.AddSingleton(s => s.GetRequiredService<IOptions<DownrOptions>>().Value);

            services.AddSingleton<IMarkdownContentLoader, MarkdownContentLoader>();
            services.AddSingleton<IYamlIndexer, YamlIndexer>();
            services.AddScoped<IPostService, PostService>();
            
            return services;
        }

        public static IServiceCollection AddDownrConstants(this IServiceCollection services)
        {
            var downrOptions = services.BuildServiceProvider().GetRequiredService<DownrOptions>();

            GlobalConstants.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            GlobalConstants.ContentPath = Path.Combine(Directory.GetCurrentDirectory(), downrOptions.ContentRoot);

            GlobalConstants.Title = downrOptions.Title;
            GlobalConstants.Author = downrOptions.Author;
            GlobalConstants.Description = downrOptions.Description;

            GlobalConstants.Version = "5.0.0";

            return services;
        }

        public static IServiceCollection AddDownr(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.AddOptions();
            services.AddMemoryCache();
            services.AddResponseCompression(c => c.Providers.Add<GzipCompressionProvider>());

            services.AddDownrConfigurations(configuration);
            services.AddDownrServices();
            services.AddDownrConstants();

            services.AddRazorPages(config =>
            {
                config.Conventions.AddPageRoute("/Index", "/Posts");
                config.Conventions.AddPageRoute("/Post", "/Posts/{slug}");
                config.Conventions.AddPageRoute("/Category", "/Categories/{name}");
            });

            return services;
        }

        public static IApplicationBuilder UseDownrStaticFiles(this IApplicationBuilder app)
        {
            var fileProvider = new PhysicalFileProvider(GlobalConstants.ContentPath);
            var contentProvider = new FileExtensionContentTypeProvider();

            contentProvider.Mappings.Remove(".md");
            contentProvider.Mappings.Remove(".js");
            contentProvider.Mappings.Remove(".css");

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                ContentTypeProvider = contentProvider,
                RequestPath = new PathString("/posts"),
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }
            });

            return app;
        }

        public static IApplicationBuilder UseDownrContent(this IApplicationBuilder app) => 
            app.UseMiddleware<DownrContentMiddleware>();

        public static IApplicationBuilder UseDownr(this IApplicationBuilder app)
        { 
            app.UseStaticFiles();

            app.UseDownrContent();
            app.UseDownrStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            return app;
        }
    }
}
