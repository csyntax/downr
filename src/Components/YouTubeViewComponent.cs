namespace downr.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using downr.Services.Youtube;

    [ViewComponent(Name ="YouTube")]
    public class YouTubeViewComponent : ViewComponent
    {
        private readonly YouTubeApiChannel youTubeApiChannel;

        public YouTubeViewComponent(YouTubeApiChannel youTubeApiChannel)
        {
            this.youTubeApiChannel = youTubeApiChannel;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var content = await this.youTubeApiChannel.GetVideos();

            return this.View();
        }
    }
}