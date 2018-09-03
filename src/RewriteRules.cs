namespace downr.Rules
{
    using System;

    using Microsoft.AspNetCore.Rewrite;

    internal class RedirectRequests : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            if (request.Path.Value.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                context.HttpContext.Response.Redirect("/posts.html");
            }
            else if (!request.Path.Value.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
            {
                context.HttpContext.Response.Redirect($"{request.Path.Value}.html");
            }
        }
    }

    internal class RewriteRequests : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            if (request.Path.Value.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
            {
                context.HttpContext.Request.Path = context.HttpContext.Request.Path.Value.Replace(".html", "");
            }
        }
    }
}