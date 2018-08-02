﻿namespace downr.Pages
{
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

        public void OnGet()
        {
            this.Posts = this.yamlIndexer.Metadata;
        }
    }
}