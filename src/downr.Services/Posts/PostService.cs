namespace downr.Services.Posts
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using downr.Common;
    using downr.Models;

    public class PostService : IPostService
    {
        private readonly IYamlIndexer yamlIndexer;

        public PostService(IYamlIndexer yamlIndexer) => 
            this.yamlIndexer = yamlIndexer;

        public Document[] GetPosts(string tag = null)
        {
            var posts = this.Posts.ToArray();

            if (tag != null)
            {
                posts = this.Posts.Where(p => p.Metadata.Tags.Contains(tag)).ToArray();
            }

            return posts;
        }

        public (int currentPage, Document[] posts, int pagesCount) GetPagedList(int page = 1, int perPage = 5)
        {
            var pagesCount = (int) Math.Ceiling(this.PostsCount() / (decimal)perPage);

            var posts = this.Posts
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToArray();

            return (currentPage: page, posts, pagesCount);
        }

        public int PostsCount(string tag = null)
        {
            int count = this.GetPosts().Length;

            if (tag != null)
            {
                count = this.GetPosts(tag).Length;
            }

            return count;
        }

        public Document GetBySlug(string slug) =>
            this.Posts.FirstOrDefault(x => string.Compare(x.Metadata.Slug.ToLower(), slug.ToLower()) == 0);

        public LinkedListNode<Metadata> GetPrevAndNextPosts(string slug)
        {
            //(Metadata prev, Metadata next) = (null, null);

            var node = this.Metadata.Nodes().Where(n => n.Value.Slug == slug).FirstOrDefault();

            /*(Metadata previous, Metadata next) result = (null, null);
            
            var metadata = this.Metadata.ToArray();

            int index = Array.FindIndex(metadata, x => string.Compare(x.Slug, slug) == 0);

            if (index != 0)
            {
                result.next = metadata[index - 1];
            }

            if (index != (metadata.Length - 1))
            {
                result.previous = metadata[index + 1];
            }

            return result;*/

            return node;
        }

        public string[] GetTags() => 
            this.Tags.ToHashSet().ToArray();

        public string GetTag(string name) =>
            this.Tags.FirstOrDefault(c => string.Compare(c.ToLower(), name.ToLower(), true) == 0);

        private IEnumerable<Document> Posts =>
            this.yamlIndexer.Documents.Where(m => DateTime.Compare(m.Metadata.Date, DateTime.Now) <= 0);

        private LinkedList<Metadata> Metadata
        {
            get
            {
                var metadata = this.Posts.Select(s => s.Metadata);

                return new LinkedList<Metadata>(metadata);
            }
        }

        private IEnumerable<string> Tags => this.Metadata.SelectMany(c => c.Tags).OrderBy(c => c).AsEnumerable();
    }
}
