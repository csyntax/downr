namespace downr.Services
{
    using System.Threading.Tasks;

    public interface IMarkdownContentLoader
    {
        Task<string> ContentRender(string rawContent, string slug);
    }
}