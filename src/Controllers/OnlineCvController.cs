namespace downr.Controlelrs 
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class OnlineCvController : Controller 
    {
        private readonly IHttpClientFactory clientFactory;

        public OnlineCvController(IHttpClientFactory clientFactory) 
        {
            this.clientFactory = clientFactory;
        }

        [Route("/Cv")]
        public async Task<IActionResult> Index() 
        {
            var client = this.clientFactory.CreateClient();

            var response = await client.GetAsync("https://raw.githack.com/csyntax/OnlineCV/master/index.html");
            var content = await response.Content.ReadAsStringAsync();

            content = content.Replace($"styles/", $"https://raw.githack.com/csyntax/OnlineCV/master/styles/");
            content = content.Replace($"scripts/", $"https://raw.githack.com/csyntax/OnlineCV/master/scripts/");
            content = content.Replace($"images/", $"https://raw.githack.com/csyntax/OnlineCV/master/images/");

            return this.Content(content, "text/html");
        }
    }
}