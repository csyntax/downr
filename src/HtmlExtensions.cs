namespace downr
{
    using System.Net;

    using HtmlAgilityPack;

    using downr.Models;

    public static class HtmlExtensions
    {
        public static string GetHtmlSnippet(this Document post, int snippetLength = 250, string continuationText = "...")
        {
            string text = post.Content.RemoveHtmlTags();

            if (snippetLength < text.Length)
            {
                int index = text.LastIndexOfAny(new[] { ' ', '\n', 'r', '\t' }, snippetLength);

                if (index < 0)
                {
                    index = snippetLength;
                }

                text = text.Substring(0, index).TrimEnd() + continuationText;
            }
            return text;
        }

        private static string RemoveHtmlTags(this string markup)
        {
            var doc = new HtmlDocument();

            doc.LoadHtml(markup);

            var html = doc.DocumentNode.InnerText;

            return WebUtility.HtmlDecode(html);
        }
    }
}
