namespace downr.Pages
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using downr.Models;
    using downr.Services;
    using downr.Services.Posts;

    public class CategoryModel : PageModel
    {
        private readonly IYamlIndexer yamlIndexer;
        private readonly IPostService postService;

        public CategoryModel(IYamlIndexer yamlIndexer, IPostService postService)
        {
            this.yamlIndexer = yamlIndexer;
            this.postService = postService;
        }

        public List<Document> Posts { get; private set; }

        public string Tag { get; private set; }

        public IActionResult OnGet(string name)
        {
            this.Tag = this.postService.GetCategory(name);
                
            if (this.Tag == null)
            {
                return this.RedirectToPage("./Index");
            }

            this.Posts = this.postService.GetPostsList(this.Tag);

            return this.Page();
        }
    }
}