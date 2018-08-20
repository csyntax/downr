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
            string content = string.Empty;

            using (FileStream file = File.OpenRead(path))
            {
                using (var reader = new StreamReader(file, Encoding.Default))
                {
                    content = reader.ReadToEnd().TrimStart('\r', '\n', '\t', ' ');

                    reader.Close();
                }

                file.Close();
            }

            var pipeline = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();
            var html = Markdown.ToHtml(content, pipeline);
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//img[@src]");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var src = node.Attributes["src"].Value.Replace("media/", $"/posts/{slug}/media/");

                    node.SetAttributeValue("src", src);
                    node.SetAttributeValue("class", "ui image");
                }
            }

            return htmlDoc.DocumentNode.OuterHtml;

            /*using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                

                

                

                

                

                reader.Close();

                return htmlDoc.DocumentNode.OuterHtml;
            }*/
        }
    }
}