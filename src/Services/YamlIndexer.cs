﻿namespace downr.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    using YamlDotNet.Serialization;

    using downr.Models;

    public class YamlIndexer : IYamlIndexer
    {
        private readonly ILogger<YamlIndexer> logger;
        private readonly IMarkdownContentLoader contentLoader;

        public ICollection<Metadata> Metadata { get; private set; }

        public YamlIndexer(ILogger<YamlIndexer> logger, IMarkdownContentLoader contentLoader)
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

            var list = Directory
                        .GetDirectories(contentPath)
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

                    var yaml = stringBuilder.ToString();
                    var result = deserializer.Deserialize<Dictionary<string, string>>(new StringReader(yaml));
   
                    var metadata = new Metadata
                    {
                        Slug = result["slug"],
                        Title = result["title"],
                        Date = DateTime.Parse(result["date"]),
                        Content = this.contentLoader.GetContentToRender(indexFile)
                    };

                    rdr.Close();

                    return metadata;
                }
            }

            return null;
        }
    }
}