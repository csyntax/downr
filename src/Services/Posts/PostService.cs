namespace downr.Services.Posts
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using downr.Models;

    public class PostService : IPostService
    {
        private readonly IYamlIndexer yamlIndexer;

        public PostService(IYamlIndexer yamlIndexer)
            => this.yamlIndexer = yamlIndexer;

        public List<Document> GetPostsList(string category = null)
        {
            var posts = this.GetPosts().ToList();

            if (category != null)
            {
                posts = this.GetPosts().Where(p => p.Categories.Contains(category)).ToList();
            }

            return posts; 
        }

        public (int currentPage, List<Document> posts, int pagesCount) 
            GetPagedList(int page = 1, int perPage = 5)
        {
            int pagesCount = (int) Math.Ceiling(this.PostsCount() / (decimal) perPage);

            var posts = this.GetPosts()
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();

            return (currentPage: page, posts: posts, pagesCount: pagesCount);
        }

        public int PostsCount(string category = null)
        {
            int count = this.GetPostsList().Count();

            if (category != null)
            {
                count = this.GetPostsList(category).Count();
            }

            return count;
        }

        public Document GetBySlug(string slug) => this.GetPosts().FirstOrDefault(x => x.Slug == slug);

        public (Document previous, Document next) GetPreviousAndNextPosts(string slug)
        {
            (Document previous, Document next) result = (null, null);

            var metadataArray = this.GetPosts().ToArray();

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

        public string[] GetCategories() => this.GetTags().ToArray();


        public string GetCategory(string name) => 
            this.GetTags()
                .FirstOrDefault(c => string.Compare(c.ToLower(), name.ToLower(), true) == 0);


        private IEnumerable<Document> GetPosts() => 
            this.yamlIndexer.Documents.Where(m => DateTime.Compare(m.Date, DateTime.Now) <= 0);

        private IEnumerable<string> GetTags() => 
            this.GetPosts()
                .SelectMany(c => c.Categories)
                .GroupBy(c => c)
                .Select(c => c.Key)
                .OrderBy(c => c);
    }
}
