namespace downr.Components
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewComponents;

    using downr.Services;

    [ViewComponent(Name = "TagCloud")]
    public class TagCloudComponent : ViewComponent
    {
        private readonly IYamlIndexer yamlIndexer;

        public TagCloudComponent(IYamlIndexer yamlIndexer)
        {
            this.yamlIndexer = yamlIndexer;
        }

        public ViewViewComponentResult Invoke()
        {
            string[] tags = this.yamlIndexer
                    .Metadata
                    .SelectMany(c => c.Categories)
                    .GroupBy(c => c)
                    .Select(c => c.Key)
                    .ToArray();

            return this.View(tags);
        }
    }
}