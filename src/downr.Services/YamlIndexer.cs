namespace downr.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    
    using Microsoft.Extensions.Logging;
  
    using YamlDotNet.Serialization;

    using downr.Common;
    using downr.Models;
    
    public class YamlIndexer : IYamlIndexer
    {
        private readonly ILogger logger;
        private readonly IMarkdownContentLoader contentLoader;

        public YamlIndexer(IMarkdownContentLoader contentLoader, ILogger<YamlIndexer> logger)
        {
            this.logger = logger;
            this.contentLoader = contentLoader;
        }

        public List<Document> Documents { get; private set; }

        /* public async Task IndexContentFiles(string contentPath)
             => await Task.Run(() =>
             {
                 this.logger.LogInformation("Loading post content...");
                 this.Documents = Directory
                     .GetDirectories(contentPath)
                     .Select(dir => Path.Combine(dir, "index.md"))
                     .Select(this.ParseMetadata)
                     .Select(m => m.Result)
                     .Where(m => m != null)
                     .OrderByDescending(x => x.Metadata.Date)
                     .ToList();
                 this.logger.LogInformation($"Loaded {this.Documents.Count} posts");
             });*/

        public async Task IndexContentFiles(string contentPath)
        {
            var documents = await Task.WhenAll(Directory
                     .GetDirectories(contentPath)
                     .Select(dir => Path.Combine(dir, "index.md"))
                     .Select(this.ParseMetadata));

            this.Documents = documents.OrderByDescending(x => x.Metadata.Date).ToList();
        }

        private async Task<string> FileReader(string indexFile)
        {
            using (var reader = new StreamReader(indexFile, Encoding.UTF8))
            {
                var content = await reader.ReadToEndAsync();

                reader.Close();

                return content;
            }
        }

        private async Task<Document> ParseMetadata(string indexFile)
        {
            var deserializer = new Deserializer();
            var slug = Path.GetFileName(Path.GetDirectoryName(indexFile));
            var rawContent = await this.FileReader(indexFile);
            var yamlHeader = rawContent.GetStringBetween("---", "---");
            var result = deserializer.Deserialize<Dictionary<string, string>>(yamlHeader);

            var metadata = new Metadata
            {
                Slug = slug,
                Title = result[MetaConstants.Title],
                Date = DateTime.ParseExact(result[MetaConstants.Date], "dd-MM-yyyy", CultureInfo.InvariantCulture),
                Tags = result[MetaConstants.Tags].Split(',').Select(c => c.Trim()).ToArray(),
            };

            var document = new Document
            {
                Metadata = metadata,
                Content = this.contentLoader.ContentRender(rawContent, slug)
            };

            return document;
        }
    }
}