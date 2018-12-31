namespace downr.Services
{
    using System.Collections.Generic;

    using downr.Models;
    using Microsoft.AspNetCore.Http;

    public interface IYamlIndexer
    {
        ICollection<Document> Documents { get; }

        //void IndexContentFiles(string contentPath);

        void IndexContentFiles(string contentPath, HttpContext httpContext);
    }
}