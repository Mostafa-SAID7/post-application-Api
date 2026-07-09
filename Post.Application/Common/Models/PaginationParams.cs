namespace Post.Application.Common.Models
{
    /// <summary>
    /// Centralized pagination parameters
    /// </summary>
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int GetSkip() => (PageNumber - 1) * PageSize;

        public PaginationParams WithValidation()
        {
            PageNumber = Math.Max(1, PageNumber);
            PageSize = Math.Min(Math.Max(1, PageSize), 100); // Max 100 items per page
            return this;
        }
    }
}
