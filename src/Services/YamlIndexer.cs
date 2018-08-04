namespace downr.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    using YamlDotNet.Serialization;

    using downr.Models;

    public class YamlIndexer : IYamlIndexer
    {
        private readonly ILogger<YamlIndexer> logger;
        private readonly IMarkdownContentLoader contentLoader;

        public ICollection<Metadata> Metadata { get; set; }

        public YamlIndexer(ILogger<YamlIndexer> logger, 
            IMarkdownContentLoader contentLoader)
        {
            this.logger = logger;
            this.contentLoader = contentLoader;
        }

        public void IndexContentFiles(string contentPath)
        {
            this.Metadata = LoadMetadata(contentPath);
        }

        private List<Metadata> LoadMetadata(string contentPath)
        {
            this.logger.LogInformation("Loading post content...");

            Func<string, Metadata> parseMetadata = delegate (string indexFile)
            {
                using (var reader = new StreamReader(indexFile, Encoding.UTF8))
                {
                    var deserializer = new Deserializer();
                    var line = reader.ReadLine();

                    if (line == "---")
                    {
                        line = reader.ReadLine();

                        var stringBuilder = new StringBuilder();

                        while (line != "---")
                        {
                            stringBuilder.Append(line);
                            stringBuilder.Append("\n");
                            line = reader.ReadLine();
                        }

                        var yaml = stringBuilder.ToString();
                        var result = deserializer.Deserialize<Dictionary<string, string>>(new StringReader(yaml));

                        var metadata = new Metadata
                        {
                            Slug = result["slug"],
                            Title = result["title"],
                            Date = DateTime.ParseExact(result["date"], "dd-MM-yyyy", CultureInfo.InvariantCulture),
                            Categories = result["categories"]?.Split(',').Select(c => c.Trim()).ToArray() ?? new string[0],
                            Content = this.contentLoader.RenderContent(indexFile, result["slug"])
                        };

                        reader.Close();

                        return metadata;
                    }
                }

                return null;
            };

            var list = Directory
                        .GetDirectories(contentPath)
                        .Select(dir => Path.Combine(dir, "index.md"))
                        .Select(parseMetadata)
                        .Where(m => m != null)
                        .OrderByDescending(x => x.Date)
                        .ToList();

            this.logger.LogInformation($"Loaded {list.Count} posts");

            return list;
        }
    }
}