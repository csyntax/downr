namespace downr.Services
{
    using System.Threading.Tasks;

    public interface IMarkdownContentLoader
    {
        Task<string> ContentRender(string path, string slug);
    }
}