namespace downr
{
    using System.IO;

    public static class Constants
    {
        public static readonly string contentPath = Path.Combine(Directory.GetCurrentDirectory(), "_posts");

        public static readonly string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    }
}