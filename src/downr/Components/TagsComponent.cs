namespace downr.Components
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Microsoft.Extensions.Caching.Memory;

    using downr.Services.Posts;

    [ViewComponent(Name = "Tags")]
    public class TagsComponent : ViewComponent
    {
        private readonly IPostService postService;
        private readonly IMemoryCache memoryCache;

        public TagsComponent(IMemoryCache memoryCache, IPostService postService)
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