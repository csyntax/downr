using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace downr.Pages
{
    public class PostModel : PageModel
    {
        private readonly IYamlIndexer yamlIndexer;

        public PostModel(IYamlIndexer yamlIndexer)
        {
            this.yamlIndexer = yamlIndexer;
        }

        public Metadata Article { get; private set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            this.Article = this.yamlIndexer.Metadata.FirstOrDefault(m => m.Slug == id);

            return Page();
        }
    }
}