namespace downr.Middleware.Extensions
{
    using System.Linq;

    using HtmlAgilityPack;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;
  
    public static class ViewExtensions
    {
        public static IHtmlContent Truncate(this IHtmlHelper helper, string content, int snippetLength = 250)
        {
            content = content.RemoveHtmlTags();

            if (snippetLength < content.Length)
            {
                int index = content.LastIndexOfAny(new char[] { ' ', '\n', 'r', '\t' }, snippetLength);

                if (index < 0)
                {
                    index = snippetLength;
                }

                content = content.Substring(0, index).TrimEnd();
            }
            
            return helper.Raw(content);
        }

        private static string RemoveHtmlTags(this string markup)
        {
            if (string.IsNullOrEmpty(markup))
            {
                return string.Empty;
            }

            var document = new HtmlDocument();

            document.LoadHtml(markup);

            document.DocumentNode
                .Descendants()
                .Where(n => n.Name == "pre")
                .ToList()
                .ForEach(n => n.Remove());

            return document.DocumentNode.InnerText;
        }
    }
}