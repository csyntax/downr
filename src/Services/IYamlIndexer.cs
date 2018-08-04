namespace downr.Services
{
    using System.Collections.Generic;

    using downr.Models;

    public interface IYamlIndexer
    {
        ICollection<Metadata> Metadata { get; }

        void IndexContentFiles(string contentPath);
    }
}