namespace downr.Models.Abstractions
{
    public abstract class PaginationModel : BaseModel
    {
        public virtual int CurrentPage { get; set; }

        public virtual int PagesCount { get; set; }

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