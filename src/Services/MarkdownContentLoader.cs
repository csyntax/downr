using HtmlAgilityPack;
using Markdig;
using System;
using System.IO;
using System.Text;

namespace downr.Services
{
    public class MarkdownContentLoader : IMarkdownContentLoader
    {
        public string GetContentToRender(string path, string slug)
        {
            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                var pipeline = new MarkdownPipelineBuilder()
                    .UseYamlFrontMatter()
                    .Build();

                var html = Markdown.ToHtml(reader.ReadToEnd(), pipeline);

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

                reader.Close();

                return html;
            }
        }
    }
}