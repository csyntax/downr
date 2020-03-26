namespace downr.Infrastructure.Extensions
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using AngleSharp;
    using AngleSharp.Html.Parser;
    
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;
    
    public static class RazorViewExtensions
    {
        public static IHtmlContent Truncate(this IHtmlHelper helper, string content, int snippetLength = 250)
        {
            content = content.StripHtml();

            if (snippetLength < content.Length)
            {
                int index = content.LastIndexOfAny(new char[] { '.' }, snippetLength);

                if (index < 0)
                {
                    index = snippetLength;
                }

                content = content.Substring(0, index).TrimEnd();
            }

            return helper.Raw($"{content}...");
        }

        private static string StripHtml(this string markup)
        {
            markup = Regex.Replace(markup, "\n(?![^<]*</pre>)", string.Empty);
            markup = Regex.Replace(markup, @"<img\s[^>]*>(?:\s*?</img>)?", string.Empty);

            /*
             * Regex.Replace(input, "<.*?>", string.Empty);
             * 
             */

            /*if (string.IsNullOrEmpty(markup))
            {
                throw new NullReferenceException($"String {nameof(markup)} is null!");
            }*/

            /*
             * Scope this to service
             */

            /* var config = Configuration.Default;
             var context = BrowsingContext.New(config);
             var parser = context.GetService<IHtmlParser>();
             var document = parser.ParseDocument(markup);

            // document.GetElementsByTagName("pre").ToList().ForEach(n => n.Remove());
            // document.GetElementsByTagName("img").ToList().ForEach(n => n.Remove());

             return document.Body.TextContent;*/

            return Regex.Replace(markup, "<.*?>", string.Empty);
        }
    }
}