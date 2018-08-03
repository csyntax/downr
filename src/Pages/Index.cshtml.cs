namespace downr.Pages
{
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.RazorPages;

    using downr.Models;
    using downr.Services;

    public class IndexModel : PageModel
    {
        private readonly IYamlIndexer yamlIndexer;

        public IndexModel(IYamlIndexer yamlIndexer)
        {
            this.yamlIndexer = yamlIndexer;
        }

        public List<Metadata> Posts { get; private set; }

        public string[] Tags { get; private set; }

        public void OnGet()
        {
            this.Posts = this.yamlIndexer.Metadata.ToList();

            this.Tags = this.yamlIndexer.Metadata.SelectMany(c => c.Categories).GroupBy(c => c).Select(c => c.Key).ToArray();
        }
    }
}