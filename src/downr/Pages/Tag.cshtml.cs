namespace downr.Pages
{  
    using Microsoft.AspNetCore.Mvc;

    using downr.Models;
    using downr.Models.Abstractions;

    using downr.Services.Posts;

    public class CategoryModel : BaseModel
    {
        private readonly IPostService postService;

        private int count;
        private string tag;
        private Document[] posts;

        public CategoryModel(IPostService postService) => 
            this.postService = postService;

 
        public int Count => this.count;

        public string Tag => this.tag;

        public Document[] Posts => this.posts;

        public IActionResult OnGet(string name)
        {
            this.tag = this.postService.GetTag(name);

            if (string.IsNullOrEmpty(this.tag))
            {
                return this.RedirectToPage("./Index");
            }

            this.posts = this.postService.GetPosts(this.tag);
            this.count = this.postService.PostsCount(this.tag);

            return this.Page();
        }
    }
}