namespace downr.Pages
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using downr.Models;
    using downr.Services;
    
    public class CategoryModel : PageModel
    {
        private readonly IYamlIndexer yamlIndexer;

        public CategoryModel(IYamlIndexer yamlIndexer)
        {
            this.yamlIndexer = yamlIndexer;
        }

        public IList<Document> Posts { get; private set; }

        public string Tag { get; private set; }

        public IActionResult OnGet(string name)
        {
            this.Tag = this.yamlIndexer
                .Documents
                .SelectMany(c => c.Categories)
                .GroupBy(c => c)
                .Select(c => c.Key)
                .FirstOrDefault(c => c.ToLower() == name.ToLower());

            if (this.Tag == null)
            {
                return this.RedirectToPage("./Index");
            }

            this.Posts = this.yamlIndexer.Documents.Where(p => p.Categories.Contains(this.Tag)).ToList();

            return this.Page();
        }
    }
}