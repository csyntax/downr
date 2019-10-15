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

        internal static class Metadata
        {
            internal static string Title => nameof(Document.Title).ToLower();

            internal static string Slug => nameof(Document.Slug).ToLower();

            internal static string Date => nameof(Document.Date).ToLower();

            internal static string Categories => nameof(Document.Categories).ToLower();
        }
    }
}