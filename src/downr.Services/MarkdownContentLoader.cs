namespace downr.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Markdig;
    using HtmlAgilityPack;

    public class MarkdownContentLoader : IMarkdownContentLoader
    {
        private readonly MarkdownPipeline markdownPipeline;

        public MarkdownContentLoader(MarkdownPipeline markdownPipeline)
            => this.markdownPipeline = markdownPipeline;

        public async Task<string> ContentRender(string path, string slug)
        {
            var htmlDoc = new HtmlDocument();

            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                var content = await reader.ReadToEndAsync();
                content = content.TrimStart('\r', '\n', '\t', ' ');

                var html = Markdown.ToHtml(content, this.markdownPipeline);

                htmlDoc.LoadHtml(html);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//img[@src]");

                try
                {
                    nodes.AsParallel().ForAll(node =>
                    {
                        var src = node.Attributes["src"].Value.Replace("media/", $"/posts/{slug}/media/");

                        node.SetAttributeValue("src", src);
                        node.SetAttributeValue("class", "img-fluid");
                    });
                } 
                catch (ArgumentNullException)
                {

                }

                return htmlDoc.DocumentNode.OuterHtml;
            }
        }
    }
}