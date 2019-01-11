namespace downr.Components
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Microsoft.Extensions.Caching.Memory;

    using downr.Services.Posts;

    public class CategoryViewComponent : ViewComponent
    {
        private readonly IPostService postService;
        private readonly IMemoryCache memoryCache;

        public CategoryViewComponent(IMemoryCache memoryCache, IPostService postService)
        {
            this.memoryCache = memoryCache;
            this.postService = postService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string[] categories = await this.memoryCache.GetOrCreateAsync("Categories", entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromDays(1));

                return Task.FromResult(this.postService.GetCategories());
            });

            return this.View(categories);
        }
    }
}