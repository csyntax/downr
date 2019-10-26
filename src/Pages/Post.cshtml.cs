namespace downr.Pages
{
    using downr.Models;
    using downr.Services.Posts;
    using Microsoft.AspNetCore.Mvc;

    public class PostModel : BaseModel
    {
        private readonly IPostService postService;

        public PostModel(IPostService postService)
            => this.postService = postService;

        [BindProperty]
        public Document Post { get; private set; }

        public IActionResult OnGet(string slug)
        {
            this.Post = this.postService.GetBySlug(slug);

            if (this.Post == null)
            {
                return this.RedirectToPage("./Index");
            }

            return this.Page();
        }
    }
}