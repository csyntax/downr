namespace downr.Middleware.Extensions
{
    using System;

    public static class ServiceProviderExtensions
    {
        public static T GetInstance<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}
