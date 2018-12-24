namespace downr.Middleware.Extensions
{
    using System;

    using Microsoft.AspNetCore.Http;

    public static class HttpContextExtensions
    {
        public static T RegisterForDispose<T>(this T disposable, HttpContext context) 
            where T : IDisposable
        {
            context.Response.RegisterForDispose(disposable);

            return disposable;
        }
    }
}