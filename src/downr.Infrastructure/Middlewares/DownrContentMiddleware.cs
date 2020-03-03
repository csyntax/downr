namespace downr.Infrastructure.Middlewares
{
    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Http;

    using downr.Common;
    using downr.Services;
    
    public class DownrContentMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IYamlIndexer yamlIndexer;

        public DownrContentMiddleware(RequestDelegate next, IYamlIndexer yamlIndexer)
        {
            this.yamlIndexer = yamlIndexer;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await this.yamlIndexer.IndexContentFiles(GlobalConstants.ContentPath);
            await this.next.Invoke(context);
        }
    }
}