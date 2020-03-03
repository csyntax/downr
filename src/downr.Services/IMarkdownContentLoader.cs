namespace downr.Services
{
    public interface IMarkdownContentLoader
    {
        string ContentRender(string path, string slug);
    }
}