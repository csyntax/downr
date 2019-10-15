namespace downr.Middleware
{
    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Http;

    using downr.Services;

    internal class DownrContentMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        private readonly IYamlIndexer yamlIndexer;

        public DownrContentMiddleware(RequestDelegate requestDelegate, IYamlIndexer yamlIndexer)
        {
            this.yamlIndexer = yamlIndexer;
            this.requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            await this.yamlIndexer.IndexContentFilesAsync(Constants.ContentPath);
            await this.requestDelegate.Invoke(httpContext);
        }
    }
}