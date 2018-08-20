namespace downr.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using downr.Models;
    using downr.Services.Posts;

    public class PostModel : BaseModel
    {
        private readonly IPostService postService;
        
        public PostModel(IPostService postService)
        {
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
    }
}