﻿namespace downr.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    
    using downr.Models;

    public interface IYamlIndexer
    {
        List<Document> Documents { get; set; }

        Task IndexContentFiles(string contentPath);
    }
}