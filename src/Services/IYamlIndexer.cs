namespace downr.Services
{
    using System.Collections.Generic;

    using downr.Models;

    public interface IYamlIndexer
    {
        List<Document> Documents { get; set; }

        void IndexContentFiles(string contentPath);
    }
}