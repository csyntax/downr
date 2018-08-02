namespace downr.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using downr.Models;
    using HtmlAgilityPack;
    using Microsoft.Extensions.Logging;
    using YamlDotNet.Serialization;

    public class YamlIndexer : IYamlIndexer
    {
        private readonly ILogger logger;

        public List<Metadata> Metadata { get; set; }

        public YamlIndexer(ILogger<YamlIndexer> logger)
        {
            this.logger = logger;
        }

        public void IndexContentFiles(string contentPath)
        {
            this.Metadata = LoadMetadata(contentPath);
        }

        private List<Metadata> LoadMetadata(string contentPath)
        {
            this.logger.LogInformation("Loading post content...");
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
                // make sure the file has the header at the first line
                var line = rdr.ReadLine();
                if (line == "---")
                {
                    line = rdr.ReadLine();

                    var stringBuilder = new StringBuilder();

                    // keep going until we reach the end of the header
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
                    htmlContent = FixUpImageUrls(htmlContent, slug);
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

        private static string FixUpImageUrls(string html, string slug)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//img[@src]");
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    var src = node.Attributes["src"].Value;
                    src = src.Replace("media/", string.Format("/posts/{0}/media/", slug));
                    node.SetAttributeValue("src", src);
                }
            }

            html = htmlDoc.DocumentNode.OuterHtml;

            return html;
        }
    }
}