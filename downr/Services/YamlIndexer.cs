namespace downr.Services
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    using downr.Models;
    using System.IO;
    using System.Linq;
    using YamlDotNet.Serialization;
    using System.Text;
    using System;

    public class YamlIndexer : IYamlIndexer
    {
        private readonly ILogger logger;

        public ICollection<Metadata> Metadata { get; set; } // private set

        public YamlIndexer(ILogger<YamlIndexer> logger)
        {
            this.logger = logger;
        }

        public void IndexContentFiles(string contentPath)
        {
            //this.Metadata = new HashSet<Metadata>();

            this.logger.LogInformation("Loading post content...");

            this.Metadata = LoadMetadata(contentPath);
        }

        private IList<Metadata> LoadMetadata(string contentPath)
        {
            List<Metadata> list = Directory.GetDirectories(contentPath)
                              .Select(dir => Path.Combine(dir, "index.md"))
                              .Select(ParseMetadata)
                              .Where(m => m != null)
                              .OrderByDescending(x => x.Date)
                              .ToList();

            this.logger.LogInformation("Loaded {0} posts", list.Count);

            return list;
        }

        private Metadata ParseMetadata(string indexFile)
        {
            var deserializer = new Deserializer();

            using (var rdr = File.OpenText(indexFile))
            {
                var line = rdr.ReadLine();

                if (line == "---")
                {
                    line = rdr.ReadLine();

                    var stringBuilder = new StringBuilder();

                    while (line != "---")
                    {
                        stringBuilder.Append(line);
                        stringBuilder.Append("\n");
                        line = rdr.ReadLine();
                    }

                    var htmlContent = rdr.ReadToEnd().TrimStart('\r', '\n', '\t', ' ');
                    htmlContent = Markdig.Markdown.ToHtml(htmlContent);

                    var yaml = stringBuilder.ToString();
                    var result = deserializer.Deserialize<Dictionary<string, string>>(new StringReader(yaml));

                    // convert the dictionary into a model
                    var slug = result["slug"];

                    var metadata = new Metadata
                    {
                        Slug = slug,
                        Title = result["title"],
                        Date = DateTime.Parse(result["date"]),
                        Content = htmlContent
                    };

                    return metadata;
                }
            }

            return null;
        }
    }
}