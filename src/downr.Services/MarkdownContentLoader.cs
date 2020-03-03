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

        public string ContentRender(string rawContent, string slug)
        {
            var htmlDoc = new HtmlDocument();
            var html = Markdown.ToHtml(rawContent, this.markdownPipeline);

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