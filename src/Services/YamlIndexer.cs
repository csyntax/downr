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

        public IList<Document> Documents { get; private set; }

        public Task IndexContentFiles(string contentPath) 
            => Task.Run(() => 
            {
                this.logger.LogInformation("Loading post content...");
                this.Documents = Directory
                    .GetDirectories(contentPath)
                    .Select(dir => Path.Combine(dir, "index.md"))
                    .Select(this.ParseMetadata)
                    .Select(m => m.Result)
                    .Where(m => m != null)
                    .OrderByDescending(x => x.Date)
                    .ToList();
                this.logger.LogInformation($"Loaded {this.Documents.Count} posts");
            });

        private async Task<Document> ParseMetadata(string indexFile)
        {
            using (var reader = new StreamReader(indexFile, Encoding.UTF8))
            {
                var deserializer = new Deserializer();
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

                    var metadata = new Document
                    {
                        Slug = slug,
                        Title = result[Constants.Metadata.Title],
                        Date = DateTime.ParseExact(result[Constants.Metadata.Date], "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        Categories = result[Constants.Metadata.Categories]
                            .Split(',')
                            .Select(c => c.Trim())
                            .ToArray(),
                        Content = await this.contentLoader.ContentRender(indexFile, slug)
                    };

                    return metadata;
                }

                return null;
            }
        }
    }
}