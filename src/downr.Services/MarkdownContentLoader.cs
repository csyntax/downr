namespace downr.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Markdig;
    using AngleSharp;
    using AngleSharp.Html.Parser;

    public class MarkdownContentLoader : IMarkdownContentLoader
    {
        private readonly MarkdownPipeline markdownPipeline;

        public MarkdownContentLoader(MarkdownPipeline markdownPipeline)
            => this.markdownPipeline = markdownPipeline;

        public async Task<string> ContentRender(string rawContent, string slug)
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var parser = context.GetService<IHtmlParser>();

            var html = Markdown.ToHtml(rawContent, this.markdownPipeline);
            var document = await parser.ParseDocumentAsync(html);

            document.Images.ToList().ForEach(img =>
            {
                var src = img.GetAttribute("src").Replace("media/", $"/posts/{slug}/media/");

                img.SetAttribute("src", src);
                img.SetAttribute("class", "img-fluid");
            });

            return document.Body.OuterHtml;
        }
    }
}