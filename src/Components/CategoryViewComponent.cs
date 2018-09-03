namespace downr.Components
{
    using Microsoft.AspNetCore.Mvc;

    using downr.Services.Posts;

    public class CategoryViewComponent : ViewComponent
    {
        private readonly IPostService postService;

        public CategoryViewComponent(IPostService postService)
        {
            this.postService = postService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = this.postService.GetCategories();

            return this.View(categories);
        }
    }
}