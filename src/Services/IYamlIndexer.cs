namespace downr.Services
{
    using System.Collections.Generic;

    using downr.Models;

    public interface IYamlIndexer
    {
        ICollection<Document> Documents { get; }

        void IndexContentFiles(string contentPath);
    }
}