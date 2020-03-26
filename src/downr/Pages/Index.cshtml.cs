namespace downr.Pages
{
    using Microsoft.AspNetCore.Mvc;

    using downr.Models;
    using downr.Models.Abstractions;

    using downr.Services.Posts;
    
    public class IndexModel : PaginationModel
    {
        private readonly IPostService postService;

        private Document[] posts;

        public IndexModel(IPostService postService) => 
            this.postService = postService;

        public Document[] Posts => this.posts;

        public IActionResult OnGet([FromQuery(Name = "page")] int page = 1)
        {
            var pagedPosts = this.postService.GetPagedList(page);

            this.posts = pagedPosts.posts;
            this.CurrentPage = pagedPosts.currentPage;
            this.PagesCount = pagedPosts.pagesCount;

            return this.Page();
        }
    }
}