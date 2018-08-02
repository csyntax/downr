namespace downr.Pages
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.RazorPages;

    using downr.Models;
    using downr.Services;

    public class PostModel : PageModel
    {
        private readonly IYamlIndexer yamlIndexer;
        
        public PostModel(IYamlIndexer yamlIndexer)
        {
            this.yamlIndexer = yamlIndexer;
        }

        public Metadata Article { get; private set; }

        public void OnGet(string id)
        {
            this.Article = this.yamlIndexer.Metadata.FirstOrDefault(x => x.Slug == id);
        }
    }
}