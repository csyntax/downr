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

        public ICollection<Document> Documents { get; private set; }

        public async Task IndexContentFiles(string contentPath)
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
            });

        private async Task<Document> ParseMetadata(string indexFile)
        {
            var deserializer = new Deserializer();

            using (var reader = new StreamReader(indexFile, Encoding.UTF8))
            {
                var line = await reader.ReadLineAsync();
                var slug = Path.GetFileName(Path.GetDirectoryName(indexFile));

                if (line.Equals("---"))
                {
                    line = await reader.ReadLineAsync();

                    var stringBuilder = new StringBuilder();

                    while (!line.Equals("---"))
                    {
                        stringBuilder.Append(line);
                        stringBuilder.Append('\n');

                        line = reader.ReadLine();
                    }

                    var yaml = stringBuilder.ToString();
                    var result = deserializer.Deserialize<IDictionary<string, string>>(yaml);

                    var metadata = new Metadata
                    {
                        Slug = slug,
                        Title = result[Constants.Meta.Title],
                        Date = DateTime.ParseExact(result[Constants.Meta.Date], "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        Tags = result[Constants.Meta.Tags].Split(',').Select(c => c.Trim()).ToArray(),
                    };

                    var document = new Document
                    {
                        Metadata = metadata,
                        Content = await this.contentLoader.ContentRender(indexFile, slug)
                    };

                    return document;
                }

                return null;
            }
        }
    }
}