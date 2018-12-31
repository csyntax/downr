namespace downr.Middleware.Extensions
{
    using System;

    using Microsoft.AspNetCore.Http;

    public static class HttpContextExtensions
    {
        public static T RegisterForDispose<T>(this T disposable, HttpContext httpContext) 
            where T : IDisposable
        {
            httpContext.Response.RegisterForDispose(disposable);

            return disposable;
        }
    }
}