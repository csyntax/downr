namespace downr.Pages
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using downr.Models;
    using downr.Services;

    public class IndexModel : PaginationModel
    {
        private readonly PostService postService;

        public IndexModel(PostService postService)
        {
            this.postService = postService;
        }

        public List<Document> Posts { get; private set; }

        public void OnGet([FromQuery(Name = "page")] int page = 1)
        {
            var pagedPosts = this.postService.GetPagedList(page);

            this.Posts = pagedPosts.posts;
            this.CurrentPage = pagedPosts.currentPage;
            this.PagesCount = pagedPosts.pagesCount;
        }
    }
}