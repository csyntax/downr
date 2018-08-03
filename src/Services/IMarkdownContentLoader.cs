namespace downr.Services
{
    public interface IMarkdownContentLoader
    {
        string RenderContent(string path, string slug);
    }
}