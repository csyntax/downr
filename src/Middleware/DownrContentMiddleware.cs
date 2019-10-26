namespace downr.Middleware
{
    using downr.Services;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

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
            await this.yamlIndexer.IndexContentFiles(Constants.ContentPath);
            await this.requestDelegate.Invoke(httpContext);
        }
    }
}