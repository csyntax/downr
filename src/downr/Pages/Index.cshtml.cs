namespace downr.Pages
{
    using downr.Models;
    using downr.Services.Posts;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    public class IndexModel : PaginationModel
    {
        private readonly IPostService postService;

        public IndexModel(IPostService postService)
            => this.postService = postService;

        [BindProperty]
        public ICollection<Document> Posts { get; private set; }

        public IActionResult OnGet([FromQuery(Name = "page")] int page = 1)
        {
            var pagedPosts = this.postService.GetPagedList(page);

            this.Posts = pagedPosts.posts;
            this.CurrentPage = pagedPosts.currentPage;
            this.PagesCount = pagedPosts.pagesCount;

            return this.Page();
        }
    }
}