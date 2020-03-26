namespace downr.Services.Posts
{
    using System.Collections.Generic;

    using downr.Models;

    public interface IPostService
    {
        Document[] GetPosts(string category = null);

        (int currentPage, Document[] posts, int pagesCount) GetPagedList(int page, int perPage = 5);

        int PostsCount(string category = null);

        Document GetBySlug(string slug);

        LinkedListNode<Metadata> GetPrevAndNextPosts(string slug);

        public string[] GetTags();

        public string GetTag(string name);
    }
}