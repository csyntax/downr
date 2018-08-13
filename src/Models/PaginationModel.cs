namespace downr.Models
{
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class PaginationModel : PageModel
    {
        public int CurrentPage { get; internal set; }

        public int PagesCount { get; internal set; }

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
    }
}