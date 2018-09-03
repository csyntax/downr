namespace downr.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Collections.Generic;

    using YamlDotNet.Serialization;

    using Microsoft.Extensions.Logging;

    using downr.Models;

    public class YamlIndexer : IYamlIndexer
    {
        private readonly ILogger logger;
        private readonly IMarkdownContentLoader contentLoader;

        public List<Document> Documents { get; private set; }

        public YamlIndexer(IMarkdownContentLoader contentLoader, ILogger<YamlIndexer> logger)
        {
            this.contentLoader = contentLoader;
            this.logger = logger;
            this.Documents = new List<Document>();
        }

        public void IndexContentFiles(string contentPath)
        {
            Func<string, Document> parseMetadata = delegate (string indexFile)
            {
                using (var reader = new StreamReader(indexFile, Encoding.UTF8))
                {
                    var deserializer = new Deserializer();
                    string line = reader.ReadLine();
                    string slug = Path.GetFileName(Path.GetDirectoryName(indexFile));

                    if (line.Equals("---"))
                    {
                        line = reader.ReadLine();

                        var stringBuilder = new StringBuilder();

                        while (!line.Equals("---"))
                        {
                            stringBuilder.Append(line);
                            stringBuilder.Append("\n");

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
                            Content = this.contentLoader.RenderContent(indexFile, slug)
                        };

                        reader.Close();
                    
                        return metadata;
                    }

                    return null;
                }
            };

            this.logger.LogInformation("Loading post content...");

            this.Documents = Directory
                .GetDirectories(contentPath)
                .Select(dir => Path.Combine(dir, "index.md"))
                .Select(parseMetadata)
                .Where(m => m != null)
                .OrderByDescending(x => x.Date)
                .ToList();

            this.logger.LogInformation($"Loaded {this.Documents.Count} posts");
        }
    }
}