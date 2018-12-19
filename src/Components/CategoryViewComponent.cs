namespace downr.Components
{
    using Microsoft.AspNetCore.Mvc;

    using Microsoft.Extensions.Caching.Memory;

    using downr.Services.Posts;
    using System;

    public class CategoryViewComponent : ViewComponent
    {
        private readonly IPostService postService;
        private readonly IMemoryCache memoryCache;

        public CategoryViewComponent(IMemoryCache memoryCache, IPostService postService)
        {
            this.memoryCache = memoryCache;
            this.postService = postService;
        }

        public IViewComponentResult Invoke()
        {
            string[] categories = this.memoryCache.GetOrCreate("Categories", entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromDays(1));

                return this.postService.GetCategories();
            });

            return this.View(categories);
        }
    }
}