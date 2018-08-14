namespace downr.Services.Posts
{
    using System.Collections.Generic;

    using downr.Models;

    public interface IPostService
    {
        List<Document> GetPostsList(string category = null);

        (int currentPage, List<Document> posts, int pagesCount) GetPagedList(int page, int perPage = 5);

        int PostsCount(string category = null);

        Document GetBySlug(string slug);

        (Document previous, Document next) GetPreviousAndNextPosts(string slug);

        string[] GetCategories();

        string GetCategory(string name);
    }
}