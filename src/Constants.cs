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
    }
}