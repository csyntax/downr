namespace downr.Services
{
    using Microsoft.AspNetCore.Http;

    public interface IYamlIndexer
    {
        void IndexContentFiles(string contentPath, HttpContext httpContext);
    }
}