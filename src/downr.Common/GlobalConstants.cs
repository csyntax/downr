namespace downr.Common
{
    public static class GlobalConstants
    {
        public  static string WebRootPath { get; set; }
           // => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        public static string ContentPath { get; set; }
          //  => Path.Combine(Directory.GetCurrentDirectory(), "Posts");
    }
}