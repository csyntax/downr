namespace downr.Services
{
    using System.Collections.Generic;

    using downr.Models;

    public interface IYamlIndexer
    {
        List<Metadata> Metadata { get; set; }

        void IndexContentFiles(string contentPath);
    }
}