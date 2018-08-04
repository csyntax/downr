namespace downr.Pages
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.RazorPages;

    using downr.Models;
    using downr.Services;
    

    public class PostModel : PageModel
    {
        private readonly IYamlIndexer yamlIndexer;
        
        public PostModel(IYamlIndexer yamlIndexer)
        {
            this.yamlIndexer = yamlIndexer;
        }

        public Metadata Article { get; private set; }

        public void OnGet(string slug)
        {
            this.Article = this.yamlIndexer.Metadata.FirstOrDefault(x => x.Slug == slug);
        }

        public (Metadata previous, Metadata next) GetPreviousAndNextPosts(string slug)
        {
            (Metadata previous, Metadata next) result = (null, null);

            var metadataArray = this.yamlIndexer.Metadata.ToArray();

            int index = Array.FindIndex(metadataArray, x => x.Slug == slug);

            if (index != 0)
            {
                result.next = metadataArray[index - 1];
            }

            if (index != (metadataArray.Length - 1))
            {
                result.previous = metadataArray[index + 1];
            }

            return result;
        }
    }
}