using HtmlAgilityPack;
using Markdig;
using System;
using System.IO;
using System.Text;

namespace downr.Services
{
    public class MarkdownContentLoader : IMarkdownContentLoader
    {
        public string GetContentToRender(string path)
        {
            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                var pipeline = new MarkdownPipelineBuilder()
                    .UseYamlFrontMatter()
                    .Build();

                var html = Markdown.ToHtml(reader.ReadToEnd(), pipeline);

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                html = htmlDoc.DocumentNode.OuterHtml;

                reader.Close();

                return html;
            }
        }
    }
}