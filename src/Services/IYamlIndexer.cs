namespace downr.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using downr.Models;

    public interface IYamlIndexer
    {
        ICollection<Document> Documents { get; }

        Task IndexContentFiles(string contentPath);
    }
}