namespace downr.Components
{
    using downr.Services.Posts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Threading.Tasks;

    public class TagsViewComponent : ViewComponent
    {
        private readonly IPostService postService;
        private readonly IMemoryCache memoryCache;

        public TagsViewComponent(IMemoryCache memoryCache, IPostService postService)
        {
            this.memoryCache = memoryCache;
            this.postService = postService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await this.memoryCache.GetOrCreateAsync("Tags", entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromDays(1));

                return Task.FromResult(this.postService.GetTags());
            });

            return this.View(categories);
        }
    }
}