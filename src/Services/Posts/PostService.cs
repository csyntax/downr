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

        public ICollection<Document> GetPostsList(string category = null)
        {
            var posts = this.Posts.ToList();

            if (category != null)
            {
                posts = this.Posts.Where(p => p.Metadata.Categories.Contains(category)).ToList();
            }

            return posts; 
        }

        public (int currentPage, ICollection<Document> posts, int pagesCount) 
            GetPagedList(int page = 1, int perPage = 5)
        {
            int pagesCount = (int) Math.Ceiling(this.PostsCount() / (decimal) perPage);

            var posts = this.Posts
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();

            return (currentPage: page, posts, pagesCount);
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

        public Document GetBySlug(string slug) => 
            this.Posts.FirstOrDefault(x => string.Compare(x.Metadata.Slug.ToLower(), slug.ToLower()) == 0);

        public (Metadata previous, Metadata next) GetPreviousAndNextPosts(string slug)
        {
            (Metadata previous, Metadata next) result = (null, null);

            var metadataArray = this.Metadata.ToArray();

            int index = Array.FindIndex(metadataArray, x => string.Compare(x.Slug, slug) == 0);

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

        public string[] GetTags() => this.Tags.ToArray();

        public string GetTag(string name) =>
            this.Tags.FirstOrDefault(c => string.Compare(c.ToLower(), name.ToLower(), true) == 0);

        private IEnumerable<Document> Posts =>
            this.yamlIndexer.Documents.Where(m => DateTime.Compare(m.Metadata.Date, DateTime.Now) <= 0);

        private IEnumerable<Metadata> Metadata => this.Posts.AsParallel().Select(s => s.Metadata);

        private IEnumerable<string> Tags => 
            this.Metadata
                .SelectMany(c => c.Categories)
                .GroupBy(c => c)
                .Select(c => c.Key)
                .OrderBy(c => c);
    }
}
