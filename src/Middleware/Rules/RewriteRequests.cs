namespace downr.Middleware.Rules
{
    using System;

    using Microsoft.AspNetCore.Rewrite;

    internal class RewriteRequests : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            if (request.Path.Value.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
            {
                context.HttpContext.Request.Path = context.HttpContext.Request.Path.Value.Replace(".html", String.Empty);
            }
        }
    }
}