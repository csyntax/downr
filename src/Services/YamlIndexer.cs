﻿namespace downr.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Collections.Generic;

    using YamlDotNet.Serialization;

    using downr.Models;

    public class YamlIndexer : IYamlIndexer
    {
        private readonly IMarkdownContentLoader contentLoader;

        public ICollection<Document> Documents { get; private set; }

        public YamlIndexer(IMarkdownContentLoader contentLoader)
        {
            this.contentLoader = contentLoader;
            this.Documents = new List<Document>();
        }

        public void IndexContentFiles(string contentPath)
        {
            Func<string, Document> parseMetadata = delegate (string indexFile)
            {
                using (var reader = new StreamReader(indexFile, Encoding.UTF8))
                {
                    var deserializer = new Deserializer();
                    var line = reader.ReadLine();

                    string slug = Path.GetFileName(Path.GetDirectoryName(indexFile));

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
                        var result = deserializer.Deserialize<IDictionary<string, string>>(new StringReader(yaml));

                        var metadata = new Document
                        {
                            Slug = slug,
                            Title = result[Constants.Publication.Title],
                            Date = DateTime.ParseExact(result[Constants.Publication.Date], "dd-MM-yyyy", CultureInfo.InvariantCulture),
                            Categories = result[Constants.Publication.Categories]?
                                        .Split(',')
                                        .Select(c => c.Trim())
                                        .ToArray() ?? new string[0],
                            Content = this.contentLoader.RenderContent(indexFile, slug)
                        };

                        reader.Close();

                        return metadata;
                    }
                }

                return null;
            };

            this.Documents = Directory
                .GetDirectories(contentPath)
                .Select(dir => Path.Combine(dir, "index.md"))
                .Select(parseMetadata)
                .Where(m => m != null)
                .OrderByDescending(x => x.Date)
                .ToList();
        }
    }
}