using System;
using System.Linq;
using System.Collections.Generic;

using downr.Models;

namespace downr.Services
{
    public class PostService
    {
        private readonly IYamlIndexer indexer;

        private const int PostsPerPageDefaultValue = 5;

        public PostService(IYamlIndexer indexer)
        {
            this.indexer = indexer;
        }

        public List<Document> GetPostsList()
        {
            var posts = this.GetPosts().ToList();

            return posts; 
        }

        public (int currentPage, List<Document> posts, int pagesCount) 
            GetPagedList(int page = 1, int perPage = PostsPerPageDefaultValue)
        {
            int pagesCount = (int) Math.Ceiling(this.PostsCount() / (decimal) perPage);

            var posts = this.GetPosts()
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();

            return (currentPage: page, posts: posts, pagesCount: pagesCount);
        }

        public int PostsCount() => this.indexer.Documents.Count();

        public Document GetBySlug(string slug)
        {
            return this.GetPosts().FirstOrDefault(x => x.Slug == slug);
        }

        public (Document previous, Document next) GetPreviousAndNextPosts(string slug)
        {
            (Document previous, Document next) result = (null, null);

            //var metadataArray = this.indexer.Documents.ToArray();

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

        private IEnumerable<Document> GetPosts()
        {
            return this.indexer
                .Documents
                .Where(m => DateTime.Compare(m.Date, DateTime.Now) <= 0);
        }
    }
}