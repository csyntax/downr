namespace downr.Services
{
    using System.IO;
    using System.Text;

    using Markdig;
    using HtmlAgilityPack;

    public class MarkdownContentLoader : IMarkdownContentLoader
    {
        public string RenderContent(string path, string slug)
        {
            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                var pipeline = new MarkdownPipelineBuilder()
                    .UseYamlFrontMatter()
                    .Build();

                var html = Markdown.ToHtml(reader.ReadToEnd().TrimStart('\r', '\n', '\t', ' '), pipeline);
                var htmlDoc = new HtmlDocument();

                htmlDoc.LoadHtml(html);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//img[@src]");

                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        var src = node.Attributes["src"].Value;

                        src = src.Replace("media/", string.Format("/posts/{0}/media/", slug));

                        node.SetAttributeValue("src", src);
                        node.SetAttributeValue("class", "ui image");
                    }
                }

                html = htmlDoc.DocumentNode.OuterHtml;

                reader.Close();

                return html;
            }
        }
    }
}