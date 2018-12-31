namespace downr.Middleware
{
    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Http;

    using downr.Services;

    internal sealed class DownrContentMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        private readonly IYamlIndexer yamlIndexer;

        public DownrContentMiddleware(RequestDelegate requestDelegate, IYamlIndexer yamlIndexer)
        {
            this.requestDelegate = requestDelegate;
            this.yamlIndexer = yamlIndexer;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            this.yamlIndexer.IndexContentFiles(Constants.ContentPath, httpContext);

            httpContext.Items["Posts"] = (object) this.yamlIndexer.Documents as object;

            await this.requestDelegate.Invoke(httpContext);
        }
    }
}