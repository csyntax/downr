namespace downr.Pages
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using downr.Models;
    using downr.Services.Posts;

    public class IndexModel : PaginationModel
    {
        private readonly IPostService postService;

        public IndexModel(IPostService postService)
            => this.postService = postService;

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