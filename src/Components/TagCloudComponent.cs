namespace downr.Components
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewComponents;

    using downr.Services.Posts;

    [ViewComponent(Name = "TagCloud")]
    public class TagCloudComponent : ViewComponent
    {
        private readonly IPostService postService;

        public TagCloudComponent(IPostService postService)
        {
            this.postService = postService;
        }

        public ViewViewComponentResult Invoke()
        {
            string[] tags = this.postService.GetCategories();

            return this.View(tags);
        }
    }
}