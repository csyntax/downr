namespace downr.Pages
{
    using downr.Models;
    using downr.Services.Posts;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    public class CategoryModel : BaseModel
    {
        private readonly IPostService postService;

        public CategoryModel(IPostService postService)
            => this.postService = postService;

        [BindProperty]
        public ICollection<Document> Posts { get; private set; }

        [BindProperty]
        public int Count { get; private set; }

        [BindProperty]
        public string Tag { get; private set; }

        public IActionResult OnGet(string name)
        {
            this.Tag = this.postService.GetTag(name);

            if (string.IsNullOrEmpty(this.Tag))
            {
                return this.RedirectToPage("./Index");
            }

            this.Posts = this.postService.GetPostsList(this.Tag);
            this.Count = this.postService.PostsCount(this.Tag);

            return this.Page();
        }
    }
}