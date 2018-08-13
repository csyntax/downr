namespace downr.Pages
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using downr.Models;
    using downr.Services;
    using downr.Services.Posts;

    public class PostModel : PageModel
    {
        private readonly IYamlIndexer yamlIndexer;
        private readonly IPostService postService;
        
        public PostModel(IYamlIndexer yamlIndexer, IPostService postService)
        {
            this.yamlIndexer = yamlIndexer;
            this.postService = postService;
        }

        public Document Article { get; private set; }

        public IActionResult OnGet(string slug)
        {
            this.Article = this.postService.GetBySlug(slug);

            if (this.Article == null)
            {
                return this.RedirectToPage("./Index");
            }

            return this.Page();
        }

       /* public (Document previous, Document next) GetPreviousAndNextPosts(string slug)
        {
            (Document previous, Document next) result = (null, null);

            var metadataArray = this.yamlIndexer.Documents.ToArray();

            int index = Array.FindIndex(metadataArray, x => x.Slug == slug);

            if (index != 0)
            {
                result.next = metadataArray[index - 1];
            }

            if (index != (metadataArray.Length - 1))
            {
                result.previous = metadataArray[index + 1];
            }

            return result;
        }*/
    }
}