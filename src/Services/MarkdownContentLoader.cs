namespace downr.Services
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using Markdig;
    using HtmlAgilityPack;
    
    public class MarkdownContentLoader : IMarkdownContentLoader
    {
        public async Task<string> ContentRender(string path, string slug)
        {
            var pipeline = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();
            var htmlDoc = new HtmlDocument();

            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                var content = await reader.ReadToEndAsync();
                content = content.TrimStart('\r', '\n', '\t', ' ');

                var html = Markdown.ToHtml(content, pipeline);

                htmlDoc.LoadHtml(html);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//img[@src]");

                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        var src = node.Attributes["src"].Value.Replace("media/", $"/posts/{slug}/media/");

                        node.SetAttributeValue("src", src);
                        node.SetAttributeValue("class", "img-fluid");
                    }
                }

                return htmlDoc.DocumentNode.OuterHtml;

            }
        }
    }
}