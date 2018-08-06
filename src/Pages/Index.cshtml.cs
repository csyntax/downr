namespace downr.Pages
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using downr.Models;
    using downr.Services;

    public class IndexModel : PageModel
    {
        private readonly IYamlIndexer yamlIndexer;

        private const int PostsPerPageDefaultValue = 5;

        public IndexModel(IYamlIndexer yamlIndexer)
        {
            this.yamlIndexer = yamlIndexer;
        }

        public IList<Metadata> Posts { get; private set; }

        public int CurrentPage { get; private set; }

        public int PagesCount { get; private set; }

        public int NextPage
        {
            get
            {
                if (this.CurrentPage >= this.PagesCount)
                {
                    return 1;
                }

                return this.CurrentPage + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (this.CurrentPage <= 1)
                {
                    return this.PagesCount;
                }

                return this.CurrentPage - 1;
            }
        }

        public void OnGet([FromQuery(Name = "page")] int page = 1, int perPage = PostsPerPageDefaultValue)
        {
            int pagesCount = (int) Math.Ceiling(this.yamlIndexer.Metadata.Count() / (decimal) perPage);

            //this.Posts = this.yamlIndexer.Metadata.ToList();

            IList<Metadata> posts = this.yamlIndexer.Metadata.Skip(perPage * (page - 1)).Take(perPage).ToList();

            this.Posts = posts;
            this.CurrentPage = page;
            this.PagesCount = pagesCount;
        }
    }
}