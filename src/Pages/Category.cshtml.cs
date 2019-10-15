namespace downr.Pages
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using downr.Models;
    using downr.Services.Posts;

    public class CategoryModel : BaseModel
    {
        private readonly IPostService postService;

        public CategoryModel(IPostService postService) 
            => this.postService = postService;

        [BindProperty]
        public List<Document> Posts { get; private set; }

        [BindProperty]
        public int Count { get;  private set; }

        [BindProperty]
        public string Tag { get; private set; }

        public IActionResult OnGet(string name)
        {
            this.Tag = this.postService.GetCategory(name);
                
            if (this.Tag == null)
            {
                return this.RedirectToPage("./Index");
            }

            this.Posts = this.postService.GetPostsList(this.Tag);
            this.Count = this.postService.PostsCount(this.Tag);

            return this.Page();
        }
    }
}