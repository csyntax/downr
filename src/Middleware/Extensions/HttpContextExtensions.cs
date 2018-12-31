namespace downr.Middleware.Extensions
{
    using System;

    using Microsoft.AspNetCore.Http;

    public static class HttpContextExtensions
    {
        public static T RegisterForDispose<T>(this T disposable, IHttpContextAccessor httpContextAccessor) 
            where T : IDisposable
        {
            httpContextAccessor.HttpContext.Response.RegisterForDispose(disposable);

            return disposable;
        }
    }
}