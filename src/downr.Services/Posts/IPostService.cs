namespace downr.Services.Posts
{
    using System.Collections.Generic;

    using downr.Models;

    public interface IPostService
    {
        ICollection<Document> GetPostsList(string category = null);

        (int currentPage, ICollection<Document> posts, int pagesCount) GetPagedList(int page, int perPage = 5);

        int PostsCount(string category = null);

        Document GetBySlug(string slug);

        (Metadata previous, Metadata next) GetPreviousAndNextPosts(string slug);

        public string[] GetTags();

        public string GetTag(string name);
    }
}