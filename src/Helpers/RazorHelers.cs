namespace downr.Helpers
{
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using HtmlAgilityPack;

    public static class RazorHelers
    {
        public static IHtmlContent Truncate(this IHtmlHelper helper, string content)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(content);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//img");

            if (nodes != null)
            {
                for (int Index = 0; Index < nodes.Count(); Index++)
                {
                    var node = nodes[Index];
                    node.Remove();
                }
            }

            return helper.Raw(content);
        }
    }
}