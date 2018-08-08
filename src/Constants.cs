namespace downr
{
    using Path = System.IO.Path;
    using Directory = System.IO.Directory;

    using downr.Models;

    public static class Constants
    {
        public static string WebRootPath
        {
            get => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        public static string ContentPath
        {
            get => Path.Combine(Directory.GetCurrentDirectory(), "_posts");
        }

        internal static class Publication
        {
            public static string Title
            {
                get => nameof(Document.Title).ToLower();
            }

            public static string Slug
            {
                get => nameof(Document.Slug).ToLower();
            }

            public static string Date
            {
                get => nameof(Document.Date).ToLower();
            }

            public static string Categories
            {
                get => nameof(Document.Categories).ToLower();
            }
        }
    }
}