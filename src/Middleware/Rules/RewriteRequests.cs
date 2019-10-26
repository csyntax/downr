namespace downr.Middleware.Rules
{
    using Microsoft.AspNetCore.Rewrite;
    using System;

    internal class RewriteRequests : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            if (request.Path.Value.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
            {
                context.HttpContext.Request.Path = context.HttpContext.Request.Path.Value.Replace(".html", string.Empty);
            }
        }
    }
}