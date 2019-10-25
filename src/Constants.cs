namespace downr
{
    using System.IO;

    using downr.Models;

    internal static class Constants
    { 
        internal static string WebRootPath 
            => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        internal static string ContentPath 
            => Path.Combine(Directory.GetCurrentDirectory(), "Posts");

        internal static class Meta
        {
            internal static string Title => nameof(Metadata.Title).ToLower();

            internal static string Slug => nameof(Metadata.Slug).ToLower();

            internal static string Date => nameof(Metadata.Date).ToLower();

            internal static string Categories => nameof(Metadata.Categories).ToLower();
        }
    }
}